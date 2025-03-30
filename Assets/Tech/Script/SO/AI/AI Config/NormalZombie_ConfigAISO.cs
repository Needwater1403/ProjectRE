using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Normal Zombie Data", menuName = "A.I/Config AI Data/Normal Zombie")]
[InlineEditor]
public class NormalZombie_ConfigAISO : ConfigAISO
{
    #region Damage Collider

    [TabGroup("AI Data", "Stalkers")]
    [FoldoutGroup("AI Data/Stalkers/Damage Collider")]
    [Title("Damage")]
    public float physicalDamage;
    [FoldoutGroup("AI Data/Stalkers/Damage Collider")]
    public float fireDamage;
    [FoldoutGroup("AI Data/Stalkers/Damage Collider")]
    public float lightningDamage;
    [Space]
    [FoldoutGroup("AI Data/Stalkers/Damage Collider")]
    public float poiseDamage;
    [FoldoutGroup("AI Data/Stalkers/Damage Collider")]
    public float staminaDamage;

    #endregion

    #region Damage Multiplier

    [Title("Damage Multiplier")]
    [FoldoutGroup("AI Data/Stalkers/Damage Collider")]
    public float attack01DmgMultiplier;
    [FoldoutGroup("AI Data/Stalkers/Damage Collider")]
    public float attack02DmgMultiplier;

    #endregion
}
