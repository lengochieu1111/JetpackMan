using System.Collections;
using System.Collections.Generic;
using MVCS.Architecture.BaseCharacter;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace UMVCS.Architecture
{
    public abstract class MVCS_BaseController<BaseModel, BaseView, BaseService> : RyoMonoBehaviour
    {
        [Header("MVCS")]
        [SerializeField] protected BaseModel model;
        [SerializeField] protected BaseView view;
        [SerializeField] protected BaseService service;

        public BaseModel Model => model;
        public BaseView View => view;
        public BaseService Service => service;

        virtual public void SetupCharacterController(BaseModel model, BaseView view, BaseService service)
        {
            this.model = model;
            this.view = view;
            this.service = service;
        }

    }

}
