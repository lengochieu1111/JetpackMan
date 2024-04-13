using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UMVCS.Architecture
{
    public abstract class MVCS_BaseModel<BaseController> : RyoMonoBehaviour
    {
        [SerializeField] protected BaseController controller;
        public BaseController Controller => controller;

        public void SetupCharacterModel(BaseController controller)
        {
            this.controller = controller;
        }
    }
}
