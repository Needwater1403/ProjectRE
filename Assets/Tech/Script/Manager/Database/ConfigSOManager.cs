using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ConfigSOManager : MonoBehaviour
{
    public static ConfigSOManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    [Title("Player")] 
    [SerializeField] private ConfigMovementSO playerMovementConfigSO;
    [SerializeField] private ConfigStaminaConsumptionSO playerStaminaConsumptionConfigSO;
    [SerializeField] private ConfigStatsSO playerStatConfigSO;
    public ConfigMovementSO PlayerMovementConfigSO => playerMovementConfigSO; 
    public ConfigStaminaConsumptionSO PlayerStaminaConsumptionConfigSO => playerStaminaConsumptionConfigSO;
    public ConfigStatsSO PlayerStatConfigSO => playerStatConfigSO;

    [HorizontalGroup("Status Effects")]
    [Title("Instant Effects")] 
    [HideLabel]
    public InstantEffects[] instantEffectsList;
    
    [Title("Timed Effects")] 
    [HideLabel]
    public TimedEffects[] timedEffectsList;
    
    [Title("Static Effects")] 
    [HideLabel]
    public StaticEffects[] staticEffectsList;
    
    public StaticEffects GetStaticEffect(string ID)
    {
        return Array.Find(staticEffectsList, staticEffect => staticEffect.EffectID == ID);
    }
    
    public TimedEffects GetTimedEffect(string ID)
    {
        return Array.Find(timedEffectsList, timedEffect => timedEffect.EffectID == ID);
    }
    
    public InstantEffects GetInstantEffect(string ID)
    {
        return Array.Find(instantEffectsList, instantEffect => instantEffect.EffectID == ID);
    }
#if UNITY_EDITOR
    [Button("Set Status Effect ID")]
    private void GenerateStatusEffectID()
    {
        foreach (var ie in instantEffectsList)
        {
            ie.SetUpID();
            EditorUtility.SetDirty(ie);
        }
        
        foreach (var se in staticEffectsList)
        {
            se.SetUpID();
            EditorUtility.SetDirty(se);
        }
        
        foreach (var te in timedEffectsList)
        {
            te.SetUpID();
            EditorUtility.SetDirty(te);
        }
    }
#endif
    [Title("VFX")] 
    public GameObject vfx_BloodSplatter;
    
}
