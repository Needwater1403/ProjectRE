using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemAction : ScriptableObject
{
    public int actionID;
    public virtual void PerformAction(PlayerManager player, WeaponItem weaponPerformTheAction)
    {
        player._controlCombat.currentWeaponPerformingAction = weaponPerformTheAction;
    }
    
    public virtual void PerformAIAction(AICharacterAutoRangeWeaponManager aiCharacterAutoRangeWeaponManager, WeaponItem weaponPerformTheAction)
    {
        
    }
}
