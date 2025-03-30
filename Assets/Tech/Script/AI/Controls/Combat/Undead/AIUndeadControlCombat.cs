using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AIUndeadControlCombat : AICharacterControlCombat
{
    [TabGroup("Combat","Undead")]
    [Title("Colliders")]
    [SerializeField] private UndeadHand_DamageCollider rightHandDamageCollider;
    [TabGroup("Combat","Undead")]
    [SerializeField] private UndeadHand_DamageCollider leftHandDamageCollider;

    [TabGroup("Combat","Undead")]
    [Title("Damage")]
    [SerializeField] private float attack_01_DmgMultiplier;
    [TabGroup("Combat","Undead")]
    [SerializeField] private float attack_02_DmgMultiplier;

    private void Start()
    {
        isUndead = true;
        rightHandDamageCollider.SetCollider(false);
        leftHandDamageCollider.SetCollider(false);
    }
    

    public void SetDamageForAttack_01()
    {
        rightHandDamageCollider.dmgMultiplier = attack_01_DmgMultiplier;
        leftHandDamageCollider.dmgMultiplier = attack_01_DmgMultiplier;
    }
    
    public void SetDamageForAttack_02()
    {
        rightHandDamageCollider.dmgMultiplier = attack_02_DmgMultiplier;
        leftHandDamageCollider.dmgMultiplier = attack_02_DmgMultiplier;
    }

    public override void DisableAllDamageColliders()
    {
        base.DisableAllDamageColliders();
        rightHandDamageCollider.SetCollider(false);
        leftHandDamageCollider.SetCollider(false);
    }
    
    #region Animation Events

    public void EnableRightHandCollider()
    {   
        _aiCharacterManager._controlSoundFXBase.PlayAttackSFX(1f);
        rightHandDamageCollider.SetCollider(true);
    }
    
    public void DisableRightHandCollider()
    {
        rightHandDamageCollider.SetCollider(false);
    }
    
    public void EnableLeftHandCollider()
    {   
        _aiCharacterManager._controlSoundFXBase.PlayAttackSFX(1f);
        leftHandDamageCollider.SetCollider(true);
    }
    
    public void DisableLeftHandCollider()
    {
        leftHandDamageCollider.SetCollider(false);
    }

    protected override void SetAIDamageDataOnStart(ConfigAISO data)
    {
        var aiData = data as NormalZombie_ConfigAISO;
        if(aiData == null) return;
        rightHandDamageCollider.physicalDmg = aiData.physicalDamage;
        rightHandDamageCollider.fireDmg = aiData.fireDamage;
        rightHandDamageCollider.lightningDmg = aiData.lightningDamage;
        rightHandDamageCollider.poiseDmg = aiData.poiseDamage;
        rightHandDamageCollider.staminaDamage = aiData.staminaDamage;
        
        leftHandDamageCollider.physicalDmg = aiData.physicalDamage;
        leftHandDamageCollider.fireDmg = aiData.fireDamage;
        leftHandDamageCollider.lightningDmg = aiData.lightningDamage;
        leftHandDamageCollider.poiseDmg = aiData.poiseDamage;
        leftHandDamageCollider.staminaDamage = aiData.staminaDamage;
        
        attack_01_DmgMultiplier = aiData.attack01DmgMultiplier;
        attack_02_DmgMultiplier = aiData.attack02DmgMultiplier;
    }
    
    #endregion
}
