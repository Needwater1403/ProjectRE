using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ConfigAIAutoRangedWeaponProjectileDataHolder
{
    [Title("Weapon")]
    public RangedProjectileItem rangedProjectileItem;
    [Title("Reload")]
    public float reloadTime;
    [Title("Range")]
    public float minRange;
    public float maxRange;
    [Title("Time Effect")] 
    public float effectTime;
    public float effectTickTime;
}
