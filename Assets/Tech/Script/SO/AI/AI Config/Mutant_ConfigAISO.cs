using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Mutant Data", menuName = "A.I/Config AI Data/Mutant")]
[InlineEditor]
public class Mutant_ConfigAISO : ConfigAISO
{
    #region Damage Collider

    [TabGroup("AI Data", "Mutant")]
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    [Title("Axe Damage")]
    public float axePhysicalDamage;
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    public float axeFireDamage;
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    public float axeLightningDamage;
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    public float axePoiseDamage;
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    public float axeStaminaDamage;
    [Space]
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    [Title("Stomp Damage")]
    public float stompPhysicalDamage;
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    public float stompFireDamage;
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    public float stompLightningDamage;
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    public float stompPoiseDamage;
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    public float stompStaminaDamage;
    [FoldoutGroup("AI Data/Mutant/Damage Collider")]
    public float stompRadius;

    #endregion

    #region Damage Multiplier

    [Title("Damage Multiplier")]
    [FoldoutGroup("AI Data/Mutant/Damage Multiplier")]
    public float attack01DmgMultiplier;
    [FoldoutGroup("AI Data/Mutant/Damage Multiplier")]
    public float attack02DmgMultiplier;
    [FoldoutGroup("AI Data/Mutant/Damage Multiplier")]
    public float attack03DmgMultiplier;
    [FoldoutGroup("AI Data/Mutant/Damage Multiplier")]
    public float attack04DmgMultiplier;
    [FoldoutGroup("AI Data/Mutant/Damage Multiplier")]
    public float attack05DmgMultiplier;
    [Space]
    [FoldoutGroup("AI Data/Mutant/Damage Multiplier")]
    public float attackStompDmgMultiplier;
    
    #endregion
}
