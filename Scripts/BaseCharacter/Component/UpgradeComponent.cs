using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeComponent : BaseCharacterAbstract
{
    [Header("Combat Mode")]
    [SerializeField] private bool _isCombatMode;
    public bool IsCombatMode
    {
        get { return _isCombatMode; }
        private set { _isCombatMode = value; }
    }

    private void ChangeCombaMode()
    {

    }


}
