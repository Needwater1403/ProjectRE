using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInventoryManager : CharacterInventoryManager
{
    [Title("Inventory")]
    [HideLabel]
    [TabGroup("Inventory", "Base")] 
    public List<Item> inventoryItems = new List<Item>();
    
    [TabGroup("Inventory","Weapon")]
    [Title("Weapon")] 
    public WeaponItem currentWeaponItem;
    
    [TabGroup("Inventory","Weapon")]
    [Title("Quick Slots")] 
    public WeaponItem[] rightHandWeaponItemQuickSlots = new WeaponItem[5];
    [HideInInspector] public int rightHandWeaponIndex = 0;
    [HideInInspector] public int rightHandWeaponUnarmedCount;
    
    [TabGroup("Inventory","Ammo")]
    [Title("Ammo")] 
    public AmmoItem currentAmmoItem;
    [TabGroup("Inventory","Ammo")]
    public int currentAmmoAmountInMag;
    [TabGroup("Inventory","Ammo")]
    public int currentAmmoCapacity;
    [TabGroup("Inventory","Ammo")]
    public int[] ammoAmountInMagList = new int[5];
    
    [TabGroup("Inventory", "Ammo")] 
    public List<AmmoItem> ammoList = new List<AmmoItem>();
    
    [TabGroup("Inventory","Consumables")]
    [Title("Current Consumable")]
    [HideLabel]
    public ConsumableItem currentConsumable;
    
    public void AddItem(Item _item, int amount = 0)
    {
        var i = inventoryItems.Find(item => item.ID == _item.ID);
        if(i == null)
        {
            var item = Instantiate(_item);
            _item.amount = amount==0?1:amount;
            inventoryItems.Add(item);
        }
        else
        {
            if (i.itemType is ItemType.Consumable or ItemType.Ammo)
            {
                i.amount+= amount==0?1:amount;
            }
            else
            {
                var item = Instantiate(_item);
                _item.amount = 1;
                inventoryItems.Add(item);
            }
        }
    }

    public void AddAmmo(Item _item, int amount = 0)
    {
        var ammo = ammoList.Find(item => item.ID == _item.ID);
        ammo.amount += amount==0?1:amount;
        if(ammo.ID == currentAmmoItem.ID)
        {
            PlayerUIManager.Instance.hubManager.SetAmmoText(currentAmmoAmountInMag, currentAmmoItem.amount);
        }
    }
    
    public void RemoveItem(Item _item)
    {
        inventoryItems.Remove(_item);
    }

    public WeaponItem GetWeaponByID(int weaponID, int personalID)
    {
        if (weaponID == ItemDatabase.Instance.UnarmedWeaponItem.ID)
            return ItemDatabase.Instance.UnarmedWeaponItem;
        var newList = (from item in inventoryItems where item.ID == weaponID && item as WeaponItem != null select item as WeaponItem).ToList();
        return newList.FirstOrDefault(item => item.personalID == personalID);
    }
    
    public RangedProjectileItem GetProjectileByID(int projectileID)
    {
        return inventoryItems.FirstOrDefault(item => item.ID == projectileID) as RangedProjectileItem;
    }
    
    public void SetCurrentWeaponOnStart()
    {
        SetPillsOnStart();
        SetAllBulletOnStart();
        if (rightHandWeaponIndex < 0) rightHandWeaponIndex = 0;
        currentWeaponItem = rightHandWeaponItemQuickSlots[rightHandWeaponIndex];
        currentAmmoItem = GetAmmoBasedOnWeapon(currentWeaponItem as RangedWeaponItem);
    }
    
    public AmmoItem GetAmmoBasedOnWeapon(RangedWeaponItem weapon)
    {
        if(weapon == null) return null;
        var ammo = ammoList.FirstOrDefault(item => item.type == weapon.ammoType);
        return ammo;
    }
    
    private void SetAllBulletOnStart()
    {
        // SET AMMO LIST
        ItemDatabase.Instance.AddAmmoToPlayerInventory();
        
        // SET AMMO CAPACITY
        foreach (var wep in rightHandWeaponItemQuickSlots)
        {
            if(wep.weaponType != WeaponType.Mace && wep.weaponType != WeaponType.Knuckle)
            {
                var rangedWep = wep as RangedWeaponItem;
                if(rangedWep == null)
                {
                    ammoAmountInMagList[rightHandWeaponItemQuickSlots.ToList().IndexOf(wep)] = 0;
                    continue;
                }
                ammoAmountInMagList[rightHandWeaponItemQuickSlots.ToList().IndexOf(wep)] = rangedWep.ammoCapacity;
            }
        }
    }
    
    // ONLY FOR THIS DEMO => FULL GAME WILL BE HANDLED DIFFERENT
    private void SetPillsOnStart()
    {
        // SET AMMO LIST
        ItemDatabase.Instance.AddPillsToPlayerInventory();
        PlayerUIManager.Instance.hubManager.SetPillsText(currentConsumable.amount);
    }
}
