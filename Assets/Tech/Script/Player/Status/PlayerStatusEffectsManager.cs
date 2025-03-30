using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerStatusEffectsManager : CharacterStatusEffectsManager
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void HandleInstantEffect(InstantEffects instantEffects)
    {
        base.HandleInstantEffect(instantEffects);
    }
    
    public override void HandleTimedEffect(TimedEffects effect)
    {
        base.HandleTimedEffect(effect);
    }
    
    public override void HandleStaticEffect(StaticEffects staticEffects, bool isRemoved)
    {
        base.HandleStaticEffect(staticEffects, isRemoved);
    }

    #region VFX
    public override void PlayItemVFX()
    {
        if (vfx_Item != null)
        {
            var vfx = Instantiate(vfx_Item, PlayerManager.Instance._controlEquipment.rightHandWeaponSlot.transform.position, Quaternion.identity);
        }
    }
    #endregion
}
