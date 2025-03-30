using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigTakeCriticalDamageInstantEffectSO", menuName = "Config/Status Effect/Instant Effects/Take Critical Damage Instant Effect")]
[InlineEditor]
public class TakeCriticalDamage_InstantEffect : TakeDamage_InstantEffect
{
    public override void SetUpID()
    {
        base.SetUpID();
        EffectID = Constants.InstantEffect_TakeCriticalDamage_ID;
        playDmgAnimation = false;
    }
    public override void ProcessEffect(CharacterManager characterManager)
    {
        //base.ProcessEffect(characterManager);
        if(characterManager.isDead)
        {
            Debug.Log("ALREADY DEAD");
            return;
        }
        
        // CHECK FOR INVULNERABILITY
        if(characterManager._controlCombatBase.isInvulnerable) return;
     
        // CALCULATE DAMAGE
        CalculateDamage(characterManager);
    }

    protected override void CalculateDamage(CharacterManager characterManager)
    {
        if (characterManager != null)
        {
            // CHECK FOR DAMAGE MODIFIERS
        }
        
        // APPLY TOTAL DAMAGE
        totalDmg = Mathf.RoundToInt(physicalDmg + fireDmg + lightningDmg); // TEMP
        totalDmg += Mathf.RoundToInt(totalDmg * characterManager._controlStatsBase.damageTakenPercentage / 100);
        if (totalDmg <= 0) totalDmg = 1;
        characterManager._controlCombatBase.criticalDmgTaken = totalDmg;
        
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
}
