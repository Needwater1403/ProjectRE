using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Items/Weapons/Melee Weapon")]
[InlineEditor]
public class MeleeWeaponItem : WeaponItem
{
    // RIPOSTE DAMAGE MODIFIERS 
    [TabGroup("Item", "Melee Weapon")]
    [Title("Critical Damage")]
    public float riposte_Damage_Modifier = 2.5f;
    
}
