using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ConfigTakeDamageInstantEffectSO", menuName = "Config/Status Effect/Instant Effects/Take Damage Instant Effect")]
[InlineEditor]
public class TakeDamage_InstantEffect : InstantEffects
{
    [Title("Attacker")] 
    public CharacterManager attackerCharacterManager;
    public WeaponType attackerWeaponType;

    [Title("Damage Type")] 
    public float physicalDmg = 0;  // INCLUDE SLASH - STRIKE - PIERCE DMG (TO DO)
    public float fireDmg = 0;
    public float lightningDmg = 0;
    
    protected float totalDmg = 0; // DMG AFTER CALCULATION

    [Title("Poise")] 
    public float poiseDmg;
    public bool isPoiseBroken;
    
    [Title("Animation")] 
    public bool playDmgAnimation = true;
    public bool manuallySelectDmgAnimation = false;

    [Title("Sound FX")] 
    public bool playDmgSFX = true;
    public AudioClip dmgSoundFX;  // USED ON TOP OF REGULAR SFX (EX: INFUSE WEAPON WITH ELEMENTAL BUFF)

    [Title("Direction of the damage source")]
    public float angleHitFrom;
    public Vector3 contactPoint;  // INSTANTIATE ANIMATION POSITION\

    [Title("Damage Penalty")] 
    public float damagePenalty;
    
    public override void SetUpID()
    {
        base.SetUpID();
        EffectID = Constants.InstantEffect_TakeDamage_ID;
    }
    public override void ProcessEffect(CharacterManager characterManager)
    {
        base.ProcessEffect(characterManager);
        if(characterManager == null) return;
        if(characterManager.isDead)
        {
            Debug.Log("ALREADY DEAD");
            return;
        }
        
        // CHECK FOR INVULNERABILITY
        if(characterManager._controlCombatBase.isInvulnerable) return;
        
        // CALCULATE DAMAGE
        CalculateDamage(characterManager);
        
        // CHECK DAMAGE SOURCE DIRECTION (TO DO)

        // PLAY DAMAGE ANIMATION
        if(!characterManager.isDead) PlayDirectionalDamageAnimation(characterManager);
        
        // PLAY DAMAGE VFX
        //PlayDamageVFX(characterManager);
        
        // CALCULATE STANCE DAMAGE ( CALL HERE BECAUSE OF PRIORITY NOT RANDOM)
        CalculateStanceDamage(characterManager);
    }
    
    protected virtual void CalculateDamage(CharacterManager characterManager)
    {
        if (characterManager != null)
        {
            // CHECK FOR DAMAGE MODIFIERS
        }
        totalDmg = Mathf.RoundToInt(physicalDmg + fireDmg + lightningDmg); // TEMP
        totalDmg += Mathf.RoundToInt(totalDmg * characterManager._controlStatsBase.damageTakenPercentage / 100);
        
        totalDmg *= 1 - damagePenalty/100;
        if (totalDmg <= 0) totalDmg = 1;
        characterManager._controlStatsBase.SetHealth(-totalDmg);
        Debug.Log($"<color=teal>{characterManager.gameObject.name}</color> received <color=orange>{totalDmg}</color> damage. " +
                  $"Remaining HP: <color=red>{characterManager._controlStatsBase.currentHealth}</color>!");
        
        // CALCULATE POISE DAMAGE
        characterManager._controlStatsBase.totalPoiseDmgTaken -= poiseDmg;
        var remainingPoise = characterManager._controlStatsBase.poiseDefense +
                             characterManager._controlStatsBase.poiseBonus +
                             characterManager._controlStatsBase.totalPoiseDmgTaken;
        
        characterManager._controlCombatBase.previousPoiseDamage = poiseDmg;
        
        if (remainingPoise <= 0)
        {
            isPoiseBroken = true;
            characterManager._controlStatsBase.poiseResetTimer =
                characterManager._controlStatsBase.defaultPoiseResetTimer;
        }
    }
    
    private void CalculateStanceDamage(CharacterManager characterManager)
    {
        var aiManager = characterManager as AICharacterManager;
        if(aiManager!=null) aiManager._controlCombat.TakeStanceDamage(Mathf.RoundToInt(poiseDmg));
    }
    
    protected void PlayDamageVFX(CharacterManager characterManager)
    {
        characterManager._controlStatusEffects.PlayBloodSplatterVFX(contactPoint);
    }
    
    
    private void PlayGruntSFX(CharacterManager characterManager)
    {
        
    }

    private void PlayDirectionalDamageAnimation(CharacterManager characterManager)
    {
        if(!playDmgAnimation) return;
        if(!isPoiseBroken)
        {
            switch (angleHitFrom)
            {
                case >= 145 and <= 180:
                // FRONT
                case <= -145 and >= -180:
                    // FRONT
                    characterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Hit_Forward_Ping_01, 
                        false, false, true, true);
                    break;
                case >= -45 and <= 45:
                    // BACK
                    characterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Hit_Back_Ping_01, 
                        false, false, true, true);
                    break;
                case >= -144 and <= -45:
                    // LEFT
                    characterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Hit_Left_Ping_01, 
                        false, false, true, true);
                    break;
                case >= 45 and <= 144:
                    // RIGHT
                    characterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Hit_Right_Ping_01, 
                        false, false, true, true);
                    break;
            }
        }
        else
        {
            switch (angleHitFrom)
            {
                case >= 145 and <= 180:
                // FRONT
                case <= -145 and >= -180:
                    // FRONT
                    characterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Hit_Forward_Medium_01, true);
                    break;
                case >= -45 and <= 45:
                    // BACK
                    characterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Hit_Back_Medium_01, true);
                    break;
                case >= -144 and <= -45:
                    // LEFT
                    characterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Hit_Left_Medium_01, true);
                    break;
                case >= 45 and <= 144:
                    // RIGHT
                    characterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Hit_Right_Medium_01, true);
                    break;
            }

            characterManager._controlCombatBase.CancelAllAttemptAction();
        }
    }
}
