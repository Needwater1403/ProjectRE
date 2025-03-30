using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TimedEffects : ScriptableObject
{
    [Title("Base Info")]
    public string EffectID;
    public float time;
    public bool isTick;
    [ShowIf("isTick")] 
    public float tickTime;
    public virtual void ProcessEffect(CharacterManager characterManager)
    {
        
    }
    
    public virtual void RemoveEffect(CharacterManager characterManager)
    {
        
    }
    
    public virtual void InitVFX(CharacterManager characterManager)
    {
        
    }
    
    public virtual void SetUpID()
    {
        
    }

    
}
