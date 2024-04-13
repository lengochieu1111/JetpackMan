using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UMVCS.Architecture;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MVCS.Architecture.BaseCharacter
{
    public class BaseCharacterController : MVCS_BaseController<BaseCharacterModel, BaseCharacterView, BaseCharacterService>
    {
        [Header("Component")]
        [SerializeField] protected BaseCharacter character;
        [SerializeField] protected CapsuleComponent capsuleComponent;
        [SerializeField] protected MovementComponent movementComponent;
        [SerializeField] protected HealthComponent healthComponent;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] protected CapsuleCollider2D capsuleCollider;

        [SerializeField] private bool _isLanding;
        [SerializeField] private bool _isExploded;
        [SerializeField] private bool _hasTouchedTheGround;
        [SerializeField] private bool _isComplytelyDead;
        [SerializeField] private bool _isEndGame;

        private Coroutine _deadCoroutine;
        [SerializeField] private float _deadCoroutineTime = 2f;

        public BaseCharacter Character => character;
        public CapsuleComponent CapsuleComponent => capsuleComponent;
        public MovementComponent MovementComponent => movementComponent;
        public HealthComponent HealthComponent => healthComponent;
        public Rigidbody2D Rigidbody => _rigidbody;
        public CapsuleCollider2D CapsuleCollider => capsuleCollider;
        public bool IsLanding
        {
            get { return this._isLanding; }
            private set { this._isLanding = value; }
        }
        public bool IsExploded
        {
            get { return this._isExploded; }
            private set { this._isExploded = value; }
        }
        public bool HasTouchedTheGround
        {
            get { return this._hasTouchedTheGround; }
            private set { this._hasTouchedTheGround = value; }
        }
        public bool IsComplytelyDead
        {
            get { return this._isComplytelyDead; }
            private set { this._isComplytelyDead = value; }
        }
        public bool IsEndGame
        {
            get { return this._isEndGame; }
            private set { this._isEndGame = value; }
        }

        /*
         * Service
        */
        public bool IsPressingRunButton => this.Service.IsPressingRunButton;
        public bool IsPressFlyButton => this.Service.IsPressingFlyButton;

        /*
         * Capsule
        */
        public bool IsOnGround => this.CapsuleComponent.IsOnGround;
        public bool IsOnCeiling => this.CapsuleComponent.IsOnCeiling;

        /*
         * Movement
        */
        public bool IsRunning => this.MovementComponent.IsRunning;
        public bool IsFlying => this.MovementComponent.IsFlying;
        public bool IsRushingUpHigh => this.MovementComponent.IsRushingUpHigh;

        /*
         * Health
        */
        public bool IsDead => this.HealthComponent.IsDead;

        #region Request
        public void SendRunRequest(bool isPressingRunButton)
        {
            if (this.IsDead) return;

            this.MovementComponent?.RequestRun(isPressingRunButton);
            this.View?.RequestRun(isPressingRunButton);
            this.IsLanding = true;
        }

        public void SendFlyRequest(bool isPressingFlyButton)
        {
            if (this.IsDead) return;

            this.MovementComponent?.RequestFly(isPressingFlyButton);
            this.View?.RequestFly(isPressingFlyButton);
            this.IsLanding = false;
        }

        public void RequestTakeDamage(float damage)
        {
            if (this.IsDead) return;

            this.HealthComponent?.TakeDamage(damage);
        }

        public void RequestDead()
        {
            if (this.IsDead) return;

            this.MovementComponent?.RequestDead();
            this.View?.RequestDead();

            this.HandleDeathEvent();

        }

        #endregion

        #region LoadComponents
        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadCharacter();

            this.LoadRigidbody();
            this.LoadCapsuleCollider();
            this.LoadCapsuleComponent();
            this.LoadMovementComponent();
            this.LoadHealthComponent();
        }

        protected virtual void LoadCharacter()
        {
            if (this.character != null) return;

            this.character = GetComponentInParent<BaseCharacter>();
        }
        
        protected virtual void LoadMovementComponent()
        {
            if (this.movementComponent != null) return;

            this.movementComponent = this.Character?.MovementComponent;
        }

        protected virtual void LoadCapsuleComponent()
        {
            if (this.capsuleComponent != null) return;

            this.capsuleComponent = this.Character ?.CapsuleComponent;
        }

        protected virtual void LoadHealthComponent()
        {
            if (this.healthComponent != null) return;

            this.healthComponent = this.Character?.HealthComponent;
        }

        protected virtual void LoadRigidbody()
        {
            if (this._rigidbody != null) return;

            this._rigidbody = this.Character?.Rigidbody;
        }
        
        protected virtual void LoadCapsuleCollider()
        {
            if (this.capsuleCollider != null) return;

            this.capsuleCollider = this.Character?.CapsuleCollider;
        }
        #endregion

        protected override void SetupValues()
        {
            base.SetupValues();

            this.IsLanding = false;
            this.IsExploded = false;
            this.HasTouchedTheGround = false;
            this.IsComplytelyDead = false;
            this.IsEndGame = false;
        }

        private void Update()
        {
            if (this.IsEndGame) return;

            this.LadingCheck();

            this.HandlesProcessOfPlayerDyingCompletely();

        }
        private void FixedUpdate()
        {
            this.HandlesTheAscentAndDescent();
        }


        /*
         * 
         */

        public void PrepareToStartMatch()
        {

        }

        public void StartMatch()
        {
            this.Rigidbody?.AddForce(new Vector2(150f, 15f));
            this.Service?.StartMatch();
        }

        public void RevivePlayer()
        {
            this.IsLanding = false;
            this.IsExploded = false;
            this.HasTouchedTheGround = false;
            this.IsComplytelyDead = false;
            this.IsEndGame = false;

            this.HealthComponent?.Revive();
            this.View?.Revive();
            this.Rigidbody?.AddForce(new Vector2(100f, 500f));
            this.Service?.StartMatch();
        }

        #region Death
        private void HandleDeathEvent()
        {
            GameManager.Instance.HandlePlayerDeath();

            this.PlayExplosionSound();
            this.PlayExplosionEffect();

            this.Rigidbody.AddForce(new Vector2(350f, 350f));
        }

        private void HandlesProcessOfPlayerDyingCompletely()
        {
            if (this.IsExploded == false && this.IsDead && this.IsOnGround == false)
            {
                this.IsExploded = true;
            }

            if (this.IsDead && this.IsExploded && this.IsOnGround && this.HasTouchedTheGround == false)
            {
                this.HandleTouchTheGroundWhenDead();
                this.HasTouchedTheGround = true;
            }

            if (this.IsDead && this.IsExploded && this.IsOnGround == false
                && this.HasTouchedTheGround && this.IsComplytelyDead == false)
            {
                this.IsComplytelyDead = true;
            }

            if (this.IsDead && this.IsExploded && this.IsOnGround
            && this.HasTouchedTheGround && this.IsComplytelyDead)
            {
                this._deadCoroutine = StartCoroutine(this.DeadCoroutine());
            }
        }

        private void HandleTouchTheGroundWhenDead()
        {
            this.Rigidbody.velocity = new Vector2(10f, 5f);
            this.Rigidbody.AddForce(new Vector2(100f, 20f));
        }

        private IEnumerator DeadCoroutine()
        {
            this.IsEndGame = true;
            this.View?.RequestComplytelyDead(true);

            float elapsedTime = 0;
            float current_X_Axis = this.Rigidbody.velocity.x;
            float current_Y_Axis = this.Rigidbody.velocity.y;

            while (elapsedTime < this._deadCoroutineTime)
            {
                elapsedTime += Time.deltaTime;

                this.Rigidbody.velocity = new Vector2(
                    Mathf.Lerp(current_X_Axis, 0, elapsedTime / this._deadCoroutineTime),
                    Mathf.Lerp(current_Y_Axis, 0, elapsedTime / this._deadCoroutineTime));

                yield return null;
            }

            GameManager.Instance.SetMatchState(MatchState.WaitingToRevived);

        }
        #endregion

        #region Landing
        private void LadingCheck()
        {
            if (this.IsFlying == true && this.IsLanding == false 
                && this.IsPressFlyButton == false && this.IsOnGround == true && this.IsOnCeiling == false)
            {
                this.CapsuleComponent?.RequestLanding();
                this.MovementComponent?.RequestRun(true);
                this.View?.RequestLanding(true);
            }
        }
        #endregion

        #region Fly
        private void HandlesTheAscentAndDescent()
        {
            if (this.IsRushingUpHigh)
            {
                if (this.IsOnCeiling)
                {
                    this.Rigidbody.velocity = new Vector2(this.Rigidbody.velocity.x, 0);
                }
                else
                {
                    this.Rigidbody.AddForce(Vector2.up * (10 + Time.fixedDeltaTime * 10));
                }
            }
            else
            {
                if (!this.IsOnGround)
                {
                    this.Rigidbody.AddForce(new Vector2(0, -Time.fixedDeltaTime * 400));
                }
            }
        }
        #endregion

        #region Sound & Effect
        public void PlayFootstepSound()
        {
            SoundManager.Instance.AudioSource.volume = 0.06f;
            SoundManager.Instance.PlayAudio(this.Model?.CharacterSO?.FootstepSound);
        }

        private void PlayExplosionSound()
        {
            SoundManager.Instance.AudioSource.volume = 0.8f;
            SoundManager.Instance.PlayAudio(this.Model?.CharacterSO?.ExplosionSound);
        }

        private void PlayExplosionEffect()
        {
            Transform effect = EffectSpawner.Instance.Spawn(EffectSpawner.Explosion_1, this.transform.position, this.transform.rotation);
            effect.gameObject.SetActive(true);
        }
        #endregion


    }
}
