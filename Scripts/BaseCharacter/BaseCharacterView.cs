using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UMVCS.Architecture;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace MVCS.Architecture.BaseCharacter
{
    public class BaseCharacterView : MVCS_BaseView<BaseCharacterController>
    {
        #region PROPERTY
        [Header("Component")]
        [SerializeField] private CharacterSO _characterSO;
        [SerializeField] protected Animator animator;

        [Header("Sound")]
        [SerializeField] private AudioClip _footstepSound;
        public Animator Animator => this.animator;
        public CharacterSO CharacterSO
        {
            get { return _characterSO; }
            private set
            {
                if (value)
                {
                    this.FootstepSound = value.FootstepSound;
                }

                _characterSO = value;
            }
        }
        public AudioClip FootstepSound
        {
            get { return this._footstepSound; }
            private set { this._footstepSound = value; }
        }
        #endregion

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadAnimator();
        }

        protected virtual void LoadAnimator()
        {
            if (this.animator != null) return;

            this.animator = GetComponent<Animator>();
        }

        public override void SetupCharacterView(BaseCharacterController controller)
        {
            base.SetupCharacterView(controller);

            this.CharacterSO = this.Controller?.Model?.CharacterSO;

        }

        public void Revive()
        {
            this.Animator.SetBool(AnimationString.isDead, false);
            this.Animator.SetBool(AnimationString.isComplytelyDead, false);
        }

        #region Run
        public void RequestRun(bool isRunning)
        {
            this.RunAnimation(isRunning);
        }

        private void RunAnimation(bool isRunning)
        {
            this.Animator.SetBool(AnimationString.isRunning, isRunning);
        }

        // Call in animation
        private void AE_PlayFootstep()
        {
            this.Controller?.PlayFootstepSound();
        }
        #endregion

        #region Fly
        public void RequestFly(bool isFlying)
        {
            if (isFlying)
            {
                this.FlyAnimation(true);
            }
        }

        private void FlyAnimation(bool isFlying)
        {
            this.Animator.SetBool(AnimationString.isFlying, isFlying);
        }
        #endregion

        #region Landing
        public void RequestLanding(bool isLanding)
        {
            if (isLanding)
            {
                this.FlyAnimation(false);
            }
        }
        #endregion

        #region Dead
        public void RequestDead()
        {
            this.Animator.SetBool(AnimationString.isRunning, false);
            this.Animator.SetBool(AnimationString.isFlying, false);
            this.Animator.SetBool(AnimationString.isDead, true);
        }
        
        public void RequestComplytelyDead(bool isDead)
        {
            this.Animator.SetBool(AnimationString.isComplytelyDead, isDead);
        }
        #endregion

    }
}
