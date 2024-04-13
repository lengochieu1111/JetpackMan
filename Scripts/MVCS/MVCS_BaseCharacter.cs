using MVCS.Architecture.BaseCharacter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UMVCS.Architecture
{
    public abstract class MVCS_BaseCharacter<BaseModel, BaseController, BaseView, BaseService> : RyoMonoBehaviour
        where BaseModel : MVCS_BaseModel<BaseController>
        where BaseView : MVCS_BaseView<BaseController>
        where BaseService : MVCS_BaseService<BaseController>
        where BaseController : MVCS_BaseController<BaseModel, BaseView, BaseService>

    {
        [Header("MVCS")]
        [SerializeField] protected BaseModel model;
        [SerializeField] protected BaseController controller;
        [SerializeField] protected BaseView view;
        [SerializeField] protected BaseService service;

        public BaseModel Model => model;
        public BaseController Controller => controller;
        public BaseView View => view;
        public BaseService Service => service;

        #region LoadComponent

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadModel();
            this.LoadController();
            this.LoadView();
            this.LoadService();
        }

        private void LoadController()
        {
            if (this.controller != null) return;

            this.controller = GetComponentInChildren<BaseController>();
        }

        protected virtual void LoadService()
        {
            if (this.model != null) return;

            this.model = GetComponentInChildren<BaseModel>();
        }

        protected virtual void LoadView()
        {
            if (this.view != null) return;

            this.view = GetComponentInChildren<BaseView>();
        }

        protected virtual void LoadModel()
        {
            if (this.service != null) return;

            this.service = GetComponentInChildren<BaseService>();
        }

        #endregion

        #region Setup
        protected override void SetupComponents()
        {
            base.SetupComponents();

            if (this.Controller && this.Model && this.View && this.Service)
            {
                this.Controller.SetupCharacterController(this.Model, this.View, this.Service);
                this.Model.SetupCharacterModel(this.Controller);
                this.View.SetupCharacterView(this.Controller);
                this.Service.SetupCharacterService(this.Controller);
            }
        }
        #endregion


    }
}
