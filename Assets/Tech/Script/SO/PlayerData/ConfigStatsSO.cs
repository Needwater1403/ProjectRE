using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigStatsSO", menuName = "Player/Config Stats")]
[InlineEditor]
public class ConfigStatsSO : ScriptableObject
{
    [Title("Attribute")]
    public float maxHealth;
    public float maxStamina;
}
