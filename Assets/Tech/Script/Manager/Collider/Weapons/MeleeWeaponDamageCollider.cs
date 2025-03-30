using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [TabGroup("Damage Collider", "Melee Weapon")] [Title("Collider Owner")]
    public CharacterManager _ownerCharacterManager;

    [Title("Weapon Attack Modifier")] 
    [TabGroup("Damage Collider", "Melee Weapon")]
    public float lightAttack01_Damage_Modifier;
    [TabGroup("Damage Collider", "Melee Weapon")]
    public float lightAttack02_Damage_Modifier;
    [TabGroup("Damage Collider", "Melee Weapon")]
    public float riposte_Damage_Modifier;
    
    private WeaponType weaponType;
    protected override void Awake()
    {
        base.Awake();
        collider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponentInParent<CharacterManager>();
        if (target != null)
        {
            if (target == _ownerCharacterManager) return;
            // CHECK FRIENDLY FIRE
            if (!WorldUltilityManager.CheckIfCharacterCanDamageAnother(
                    _ownerCharacterManager._controlCombatBase.charactersGroup,
                    target._controlCombatBase.charactersGroup)) return;

            // CHECK IF THE TARGET IS INVULNERABLE
            if (target._controlCombatBase.isInvulnerable) return;

            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            
            // APPLY DAMAGE
            ApplyDamage(target);
        }
    }

    protected override void ApplyDamage(CharacterManager target)
    {
        if (targetDamagedList.Contains(target)) return;
        targetDamagedList.Add(target);
        // ALWAYS INSTANTIATE NEW SCRIPTABLE OBJECT TO AVOID CHANGING THE ORIGINAL VALUE
        var effect =
            Instantiate(
                ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeDamage_ID) as
                    TakeDamage_InstantEffect);
        effect.attackerCharacterManager = _ownerCharacterManager;
        effect.attackerWeaponType = weaponType;
        effect.physicalDmg = physicalDmg;
        effect.fireDmg = fireDmg;
        effect.lightningDmg = lightningDmg;
        effect.poiseDmg = poiseDmg;

        effect.contactPoint = contactPoint;
        effect.angleHitFrom =
            Vector3.SignedAngle(_ownerCharacterManager.transform.forward, target.transform.forward, Vector3.up);

        switch (_ownerCharacterManager._controlCombatBase.currentMeleeWeaponAttackType)
        {
            case MeleeWeaponAttackType.LightAttack01:
                ApplyDamageModifier(lightAttack01_Damage_Modifier, effect);
                break;
            case MeleeWeaponAttackType.LightAttack02:
                ApplyDamageModifier(lightAttack02_Damage_Modifier, effect);
                break;
        }

        // APPLY DAMAGE
        target._controlStatusEffects.HandleInstantEffect(effect);
    }

    private static void ApplyDamageModifier(float modifier, TakeDamage_InstantEffect takeDamageInstantEffect,
        WeaponItem weaponItem = null)
    {
        takeDamageInstantEffect.physicalDmg *= modifier;
        takeDamageInstantEffect.fireDmg *= modifier;
        takeDamageInstantEffect.lightningDmg *= modifier;

        takeDamageInstantEffect.poiseDmg *= modifier;
        // HAVE ANOTHER MODIFIER IF USING CHARGE ATTACK (TO DO)
    }

    public void SetDamageToMeleeWeaponCollider(WeaponItem weaponItem)
    {
        weaponType = weaponItem.weaponType;
        
        physicalDmg = weaponItem.physicalDamage;
        fireDmg = weaponItem.fireDamage;
        lightningDmg = weaponItem.lightningDamage;
            
        poiseDmg = weaponItem.poiseDamage;
        staminaDamage = weaponItem.staminaDamage;
            
        lightAttack01_Damage_Modifier = weaponItem.lightAttack01_Damage_Modifier;
        lightAttack02_Damage_Modifier = weaponItem.lightAttack02_Damage_Modifier;
            
        var meleeWeaponItem = weaponItem as MeleeWeaponItem;
        if (meleeWeaponItem == null) return;
        riposte_Damage_Modifier = meleeWeaponItem.riposte_Damage_Modifier;
    }
}
