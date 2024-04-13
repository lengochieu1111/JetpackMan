using System;
using System.Collections;
using System.Collections.Generic;
using UMVCS.Architecture;
using UnityEngine;

namespace MVCS.Architecture.BaseCharacter
{
    public class BaseCharacterModel : MVCS_BaseModel<BaseCharacterController>
    {
        [SerializeField] private CharacterSO characterSO;
        public CharacterSO CharacterSO => characterSO;

        #region LoadComponent

        protected override void LoadComponents()
        {
            base.LoadComponents();

            this.LoadCharacterSO();
        }
        private void LoadCharacterSO()
        {
            string resPath = "SO_" + this.transform.parent?.name;
            this.characterSO = Resources.Load<CharacterSO>(resPath);
        }

        #endregion

    }
}
