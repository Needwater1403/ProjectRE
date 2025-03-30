using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Auto Ranged Weapon Data", menuName = "A.I/Config AI Auto Ranged Weapon")]
[InlineEditor]
public class ConfigAIAutoRangedWeapon : ScriptableObject
{
    [Title("Weapon")]
    public RangedWeaponItem rangedWeaponItem;
    [Title("Ammo")]
    public List<ConfigAIAutoRangedWeaponAmmoDataHolder> ammoItemDataList;
    public List<ConfigAIAutoRangedWeaponProjectileDataHolder> rangedProjectileItemDataList;
    
}
