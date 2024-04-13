using System.Collections;
using System.Collections.Generic;
using UMVCS.Architecture;
using UnityEngine;

namespace UMVCS.Architecture
{
    public abstract class MVCS_BaseService<BaseController> : RyoMonoBehaviour
    {
        [SerializeField] protected BaseController controller;
        public BaseController Controller => controller;

        public void SetupCharacterService(BaseController controller)
        {
            this.controller = controller;
        }
    }
}
