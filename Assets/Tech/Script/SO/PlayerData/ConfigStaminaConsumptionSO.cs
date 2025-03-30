using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigStaminaConsumptionSO", menuName = "Player/Config Stamina Consumption")]
public class ConfigStaminaConsumptionSO : ScriptableObject
{
    [Title("Stamina Regen")]
    public float staminaRegenDelayTime = 1;
    public float staminaRegenAmount = 2;
    
    [Title("Stamina Cost")]
    public float staminaDodge = 25;
    public float staminaSprint = 5;
    public float staminaJump = 17;
}
