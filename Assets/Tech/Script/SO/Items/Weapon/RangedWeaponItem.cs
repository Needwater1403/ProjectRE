using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Items/Weapons/Ranged Weapon")]
[InlineEditor]
public class RangedWeaponItem : WeaponItem
{
    [TabGroup("Item", "Ranged Weapon")] 
    [Title("Ammo")]
    public AmmoType ammoType;
    [TabGroup("Item", "Ranged Weapon")] 
    public int ammoCapacity = 12;
    
    [TabGroup("Item", "Ranged Weapon")] 
    [Title("Fire Mode")]
    public FireMode fireMode;
    [TabGroup("Item", "Ranged Weapon")] 
    public float fireRate = 5f;
    [TabGroup("Item", "Ranged Weapon")] 
    public float burstFireRate = 10f;
    [Space]
    [Title("Shotgun Params")]
    [OnValueChanged("OnBulletPerShotChanged")]
    [TabGroup("Item", "Ranged Weapon")] 
    public int bulletPerShot = 6;
    [TabGroup("Item", "Ranged Weapon")] 
    public float spreadOffset = 10f;
    
    [TabGroup("Item", "Ranged Weapon")] 
    [Title("VFX")]
    public GameObject hitZombieVFX;
    [TabGroup("Item", "Ranged Weapon")] 
    public GameObject hitTerrainVFX;
    
    [TabGroup("Item", "Ranged Weapon")] 
    [Title("SFX")]
    public RarityRandomList<AudioClip> reloadSFX;
    [TabGroup("Item", "Ranged Weapon")] 
    public RarityRandomList<AudioClip> aimFX;
    [TabGroup("Item", "Ranged Weapon")] 
    public RarityRandomList<AudioClip> emptySFX;
    
    private void OnBulletPerShotChanged()
    {
        if (bulletPerShot < 1) bulletPerShot = 1;
        if(weaponType != WeaponType.Shotgun) bulletPerShot = 1;
    }
}

