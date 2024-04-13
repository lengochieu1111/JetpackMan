using System;
using System.Collections;
using System.Collections.Generic;
using UMVCS.Architecture;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

namespace MVCS.Architecture.BaseCharacter
{
    public class BaseCharacterService : MVCS_BaseService<BaseCharacterController>
    {
        #region PROPERTTY

        [Header("Player Input")]
        [SerializeField] private IMC_Jetpack_Default IMC_Jetpack;

        [Header("Input Value")]
        [SerializeField] private bool isPressingRunButton = false;
        [SerializeField] private bool isPressingFlyButton = false;
        
        public bool IsPressingRunButton
        {
            get { return isPressingRunButton; }

            private set
            {
                // Send request to controller
                this.Controller?.SendRunRequest(value);
                this.isPressingRunButton = value;
            }
        }
        
        public bool IsPressingFlyButton
        {
            get { return this.isPressingFlyButton; }

            private set
            {
                // Send request to controller
                this.Controller?.SendFlyRequest(value);
                this.isPressingFlyButton = value;
            }
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            /* Player Input */
            this.CreatePlayerInput();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this.IMC_Jetpack.Enable();

            this.IMC_Jetpack.DefaultInput.Run.performed += OnRunPerformed;
            this.IMC_Jetpack.DefaultInput.Run.canceled += OnRunCanceled;

            this.IMC_Jetpack.DefaultInput.Fly.performed += OnFlyPerformed;
            this.IMC_Jetpack.DefaultInput.Fly.canceled += OnFlyCanceled;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            this.IMC_Jetpack.Disable();

            this.IMC_Jetpack.DefaultInput.Run.performed -= OnRunPerformed;
            this.IMC_Jetpack.DefaultInput.Run.canceled -= OnRunCanceled;

            this.IMC_Jetpack.DefaultInput.Fly.performed -= OnFlyPerformed;
            this.IMC_Jetpack.DefaultInput.Fly.canceled -= OnFlyCanceled;
        }

        public void StartMatch()
        {
            this.IsPressingRunButton = true;
        }

        protected virtual void CreatePlayerInput()
        {
            if (this.IMC_Jetpack != null) return;

            this.IMC_Jetpack = new IMC_Jetpack_Default();
        }

        #region Input Action

        protected virtual void OnRunPerformed(InputAction.CallbackContext value)
        {
            this.IsPressingRunButton = true;
        }

        protected virtual void OnRunCanceled(InputAction.CallbackContext value)
        {
            this.IsPressingRunButton = false;
        }

        private void OnFlyPerformed(InputAction.CallbackContext context)
        {
            this.IsPressingFlyButton = true;
        }

        private void OnFlyCanceled(InputAction.CallbackContext context)
        {
            this.IsPressingFlyButton = false;
        }

        #endregion

    }
}
