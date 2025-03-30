using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AICharacterManager aiCharacterManager)
    {
        return this;
    }
    
    protected virtual AIState SwitchState(AICharacterManager aiCharacterManager, AIState newState)
    {
        ResetFlags(aiCharacterManager);
        return newState;
    }
    
    protected virtual bool RollForOutcomeChance(float outcomeChance)
    {
        var outcome = false;
        if (outcomeChance == 0f) return false;
        var randomPercentage = Random.Range(0, 100);
        if (randomPercentage <= outcomeChance) outcome = true;
        return outcome;
    }
    
    protected virtual void ResetFlags(AICharacterManager aiCharacterManager)
    {
        
    }
}

