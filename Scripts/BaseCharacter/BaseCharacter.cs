using MVCS.Architecture.BaseCharacter;
using System;
using System.Collections;
using System.Collections.Generic;
using UMVCS.Architecture;
using UnityEngine;
using UnityEngine.Events;

namespace MVCS.Architecture.BaseCharacter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseCharacter : MVCS_BaseCharacter
    <BaseCharacterModel, BaseCharacterController, BaseCharacterView, BaseCharacterService>
        , I_Damageable, I_PickUpItem
    {
        [Header("Component")]
        [SerializeField] protected CapsuleComponent capsuleComponent;
        [SerializeField] protected MovementComponent movementComponent;
        [SerializeField] protected HealthComponent healthComponent;
        [SerializeField] protected Rigidbody2D rigidbody_;
        [SerializeField] protected CapsuleCollider2D capsuleCollider;

        public CapsuleComponent CapsuleComponent => this.capsuleComponent;
        public MovementComponent MovementComponent => this.movementComponent;
        public HealthComponent HealthComponent => this.healthComponent;
        public Rigidbody2D Rigidbody => this.rigidbody_;
        public CapsuleCollider2D CapsuleCollider => this.capsuleCollider;

        #region LoadComponent

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadCapsuleComponent();
            this.LoadMovementComponent();
            this.LoadHealthComponent();

            this.LoadRigidbody();
            this.LoadCapsuleCollider();
        }

        protected virtual void LoadCapsuleComponent()
        {
            if (this.capsuleComponent != null) return;

            this.capsuleComponent = GetComponentInChildren<CapsuleComponent>();
        }

        protected virtual void LoadHealthComponent()
        {
            if (this.healthComponent != null) return;

            this.healthComponent = GetComponentInChildren<HealthComponent>();
        }

        protected virtual void LoadMovementComponent()
        {
            if (this.movementComponent != null) return;

            this.movementComponent = GetComponentInChildren<MovementComponent>();
        }

        private void LoadRigidbody()
        {
            if (this.rigidbody_ != null) return;

            this.rigidbody_ = GetComponent<Rigidbody2D>();
        }

        private void LoadCapsuleCollider()
        {
            if (this.capsuleCollider != null) return;

            this.capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        #endregion

        #region Setup Component
        protected override void SetupValues()
        {
            base.SetupValues();

            this.Rigidbody.gravityScale = 0.0f;
            this.Rigidbody.freezeRotation = true;
        }
        #endregion


        /* Test */
        protected override void Start()
        {
            base.Start();

            PrepareToStartMatch();
            StartMatch();
        }

        //
        public void PrepareToStartMatch()
        {
            this.Controller?.PrepareToStartMatch();
        }

        public void StartMatch()
        {
            this.Controller?.StartMatch();
        }

        public void RevivePlayer()
        {
            this.Controller?.RevivePlayer();
        }

        #region Damageable
        public bool TakeDamage(float damage)
        {
            if (this.Controller.IsDead) return false;

            this.Controller?.RequestTakeDamage(damage);
            return true;
        }
        #endregion

        #region ReceiveItem
        public void PickUp_Coin(ECoinType coinType)
        {
            GameMode.Instance.HandlePlayerPickUp_Coin(coinType);
        }
        #endregion

    }
}
