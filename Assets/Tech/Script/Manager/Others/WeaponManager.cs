using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider _damageCollider;
    public List<ParticleSystem> vfxList = new List<ParticleSystem>();
    public Transform projectileInitTf;
    private void Awake()
    {
        if(_damageCollider==null) _damageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(WeaponItem weaponItem, CharacterManager ownerCharacterManager)
    {
        if(_damageCollider==null) return;
        // SET OWNER
        _damageCollider._ownerCharacterManager = ownerCharacterManager;
        
        // SET DAMAGE
        _damageCollider.SetDamageToMeleeWeaponCollider(weaponItem);
    }

    public void PlayWeaponVFX()
    {
        foreach (var vfx in vfxList)
        {
            vfx.Play();
        }
    }
}
