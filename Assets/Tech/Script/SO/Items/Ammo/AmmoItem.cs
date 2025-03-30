using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo", menuName = "Items/Ammo")]
[InlineEditor]
public class AmmoItem : Item
{
    [TabGroup("Item","Ammo")]
    [EnumToggleButtons]
    [HideLabel]
    [Title("Type")]
    public AmmoType type;
    
    [TabGroup("Item","Ammo")]
    [Title("Capacity")]
    public int maxAmount = 30;
    
    [TabGroup("Item","Ammo")]
    [Title("Damage")]
    [TabGroup("Item","Ammo")]
    public int physicalDamage = 0;
    [TabGroup("Item","Ammo")]
    public int fireDamage = 0;
    [TabGroup("Item","Ammo")]
    public int lightningDamage = 0;
    [TabGroup("Item","Ammo")]
    public int poiseDamage = 0;
    
}