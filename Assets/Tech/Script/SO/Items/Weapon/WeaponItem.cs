using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponItem : EquipmentItem
{
    // ANIMATOR CONTROLLER OVERRIDE
    [TabGroup("Item","Weapon")]
    [Title("Animator Override")] 
    public AnimatorOverrideController animatorOverrideController;
    
    [HorizontalGroup("Item/Weapon/Game Data",75)]
    [Title("Model")] 
    [PreviewField(75)]
    [HideLabel]
    public GameObject weaponModel;
    
    [HorizontalGroup("Item/Weapon/Game Data/Weapon Type")]
    [Title("Weapon Type")] 
    [EnumToggleButtons]
    [HideLabel]
    public WeaponType weaponType;
    
    // ALL OF THESE ATTRIBUTES ARE PUT HERE SO RANGE WEAPONS (GUNS) WILL HAVE MELEE ACTION (TO DO)
    [FoldoutGroup("Item/Weapon/Damage")]
    [Title("Base Damage")] 
    [FoldoutGroup("Item/Weapon/Damage")]
    public int physicalDamage = 0;
    [FoldoutGroup("Item/Weapon/Damage")]
    public int fireDamage = 0;
    [FoldoutGroup("Item/Weapon/Damage")]
    public int lightningDamage = 0;
    [Space]
    
    [FoldoutGroup("Item/Weapon/Damage")]
    [Title("Poise Damage")] 
    public int poiseDamage = 0;
    [Space]
    
    [FoldoutGroup("Item/Weapon/Damage")]
    [Title("Stamina Damage")] 
    public int staminaDamage = 0;
    [Space]
    
    [FoldoutGroup("Item/Weapon/Damage")]
    [Title("Damage Modifier")]
    [LabelWidth(250)]
    public float lightAttack01_Damage_Modifier = 1.1f;
    [FoldoutGroup("Item/Weapon/Damage")]
    [LabelWidth(250)]
    public float lightAttack02_Damage_Modifier = 1.2f;
    
    [FoldoutGroup("Item/Weapon/Stamina Cost")]
    public float baseStaminaCost = 12f;
    [FoldoutGroup("Item/Weapon/Stamina Cost")]
    public float lightAttack01_StaminaCost_Multiplier = 1f;
    [FoldoutGroup("Item/Weapon/Stamina Cost")]
    public float lightAttack02_StaminaCost_Multiplier = 1.1f;
    
    [FoldoutGroup("Item/Weapon/SFX")]
    [Title("Attack SFX")]
    public RarityRandomList<AudioClip> AttackSFX;
    
    [LabelWidth(250)]
    [FoldoutGroup("Item/Weapon/Action")]
    public WeaponItemAction weaponItemAction;

}
