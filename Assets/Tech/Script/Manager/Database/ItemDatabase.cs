using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using VFolders.Libs;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;
    private List<Item> itemsDatabase = new List<Item>();
    
    [Title("Unarmed")]
    public WeaponItem UnarmedWeaponItem;

    [Title("Projectiles")] 
    public RangedProjectileItem grenadeItem;
    public RangedProjectileItem arrowItem;
    
    [Title("Weapons Database")] 
    [SerializeField] private List<WeaponItem> weaponItemsDatabase = new List<WeaponItem>();
    
    [FormerlySerializedAs("projectileItemsDatabase")]
    [Title("Ammo Database")] 
    [SerializeField] private List<AmmoItem> ammoItemsDatabase = new List<AmmoItem>();
    
    [Title("KeyItems Database")] 
    [SerializeField] private List<KeyItem> keyItemsDatabase = new List<KeyItem>();
    
    [Title("Consumables Database")] 
    [SerializeField] private List<ConsumableItem> consumableItemsDatabase = new List<ConsumableItem>();

    private void Awake()
    {
        SetUpWeaponDatabase();
        SetUpProjectilesDatabase();
        SetUpKeyItemsDatabase();
        SetUpConsumablesDatabase();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #region SET UP ITEMS ID
#if UNITY_EDITOR
    [Button("SET UP ALL ID")]
    private void SetUpAllID()
    {
        // WEAPONS
        for (var i = 0; i < weaponItemsDatabase.Count; i++)
        {
            weaponItemsDatabase[i].ID = Constants.Item_Weapon_ID + i;
            weaponItemsDatabase[i].level = 0;
            if(weaponItemsDatabase[i].baseName == "")
                weaponItemsDatabase[i].baseName = weaponItemsDatabase[i].name;
            weaponItemsDatabase[i].name = weaponItemsDatabase[i].baseName;
            EditorUtility.SetDirty(weaponItemsDatabase[i]);
        }
        
        // AMMO
        for (var i = 0; i < ammoItemsDatabase.Count; i++)
        {
            ammoItemsDatabase[i].ID = Constants.Item_Ammo_ID+ i;
            ammoItemsDatabase[i].amount = 1;
            EditorUtility.SetDirty(ammoItemsDatabase[i]);
        }
        
        // KEY ITEMS
        for (var i = 0; i < keyItemsDatabase.Count; i++)
        {
            keyItemsDatabase[i].ID = Constants.Item_KeyItems_ID + i;
            keyItemsDatabase[i].amount = 1;
            EditorUtility.SetDirty(keyItemsDatabase[i]);
        }
        
        // CONSUMABLES
        for (var i = 0; i < consumableItemsDatabase.Count; i++)
        {
            consumableItemsDatabase[i].ID = Constants.Item_Consumable_ID + i;
            consumableItemsDatabase[i].amount = 1;
            if(consumableItemsDatabase[i].baseName  == "")
                consumableItemsDatabase[i].baseName = consumableItemsDatabase[i].name;
            consumableItemsDatabase[i].name = consumableItemsDatabase[i].baseName;
            EditorUtility.SetDirty(consumableItemsDatabase[i]);
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif  
    private void SetUpWeaponDatabase()
    {
        // MAKE UNIQUE ID
        for (var i = 0; i < weaponItemsDatabase.Count; i++)
        {
            weaponItemsDatabase[i] = Instantiate(weaponItemsDatabase[i]);
        }
        
        // ADD TO GENERAL ITEM DATABASE
        foreach (var w in weaponItemsDatabase)
        {
            itemsDatabase.Add(w);
        }

    }
    
    private void SetUpProjectilesDatabase()
    {
        // MAKE UNIQUE ID
        for (var i = 0; i < ammoItemsDatabase.Count; i++)
        {
            ammoItemsDatabase[i] = Instantiate(ammoItemsDatabase[i]);
        }
        
        // ADD TO GENERAL ITEM DATABASE
        foreach (var w in ammoItemsDatabase)
        {
            itemsDatabase.Add(w);
        }

    }
    
    private void SetUpKeyItemsDatabase()
    {
        // MAKE UNIQUE ID
        for (var i = 0; i < keyItemsDatabase.Count; i++)
        {
            keyItemsDatabase[i] = Instantiate(keyItemsDatabase[i]);
        }
        
        // ADD TO GENERAL ITEM DATABASE
        foreach (var w in keyItemsDatabase)
        {
            itemsDatabase.Add(w);
        }
    }
    
    private void SetUpConsumablesDatabase()
    {
        // MAKE UNIQUE ID
        for (var i = 0; i < consumableItemsDatabase.Count; i++)
        {
            consumableItemsDatabase[i] = Instantiate(consumableItemsDatabase[i]);
        }
        
        // ADD TO GENERAL ITEM DATABASE
        foreach (var w in consumableItemsDatabase)
        {
            itemsDatabase.Add(w);
        }
    }
    #endregion

    #region GET ITEM BY ID

    public Item GetItemByID(int itemID)
    {
        return itemsDatabase.FirstOrDefault(item => item.ID == itemID);
    }
    public WeaponItem GetWeaponByID(int weaponID)
    {
        return weaponItemsDatabase.FirstOrDefault(weapon => weapon.ID == weaponID);
    }
    
    public AmmoItem GetAmmoByID(int projectileID)
    {
        return ammoItemsDatabase.FirstOrDefault(projectile => projectile.ID == projectileID);
    }
    
    public ConsumableItem GetConsumableByID(int consumableID)
    {
        return consumableItemsDatabase.FirstOrDefault(consumable => consumable.ID == consumableID);
    }
    #endregion

    public void AddAmmoToPlayerInventory()
    {
        foreach (var ammo in ammoItemsDatabase)
        {
            var newAmmo = Instantiate(ammo);
            newAmmo.amount = 100;
            PlayerManager.Instance._controlInventory.ammoList.Add(newAmmo);
        }
    }
    
    // TEMP FOR THIS DEMO ONL => NOT THE RIGHT WAY TO HANDLE THIS
    public void AddPillsToPlayerInventory()
    {
        PlayerManager.Instance._controlInventory.currentConsumable = Instantiate(consumableItemsDatabase[0]);
        PlayerManager.Instance._controlInventory.currentConsumable.amount = 99;
    }
}
