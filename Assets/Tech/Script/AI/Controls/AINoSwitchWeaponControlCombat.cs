using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINoSwitchWeaponControlCombat : AICharacterControlCombat
{
    private AICharacterManager _playerManager;
    [SerializeField] private WeaponManager rightHandWeaponManager;

    protected override void Awake()
    {
        base.Awake();
        //if(rightHandWeaponManager != null) rightHandWeaponManager.SetWeaponDamage();
    }
    
}
