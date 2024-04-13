using System.Collections;
using System.Collections.Generic;
using UMVCS.Architecture;
using UnityEngine;

namespace UMVCS.Architecture
{
    public abstract class MVCS_BaseView<BaseController> : RyoMonoBehaviour
    {
        [SerializeField] protected BaseController controller;
        public BaseController Controller => controller;

        public virtual void SetupCharacterView(BaseController controller)
        {
            this.controller = controller;
        }
    }
}