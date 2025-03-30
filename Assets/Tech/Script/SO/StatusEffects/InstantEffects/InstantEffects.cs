using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InstantEffects : ScriptableObject
{
    [Title("Base Info")]
    public string EffectID;
    
    public virtual void ProcessEffect(CharacterManager characterManager)
    {
        
    }
    
    public virtual void SetUpID()
    {
        
    }
}
