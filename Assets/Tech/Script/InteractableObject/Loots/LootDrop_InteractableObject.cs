using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class LootDrop_InteractableObject : InteractableObject
{
    [TabGroup("Interactive","Loot")]
    [VerticalGroup("Interactive/Loot/Data/Item Info")]
    [Title("Item Info")] 
    public Item item;
    [VerticalGroup("Interactive/Loot/Data/Item Info")]
    public int amount;
    [VerticalGroup("Interactive/Loot/Data/Item Info")]
    public bool isLooted;
    [VerticalGroup("Interactive/Loot/Data/Item Info")]
    public bool destroyObjectOnLoot;
    
    [HorizontalGroup("Interactive/Loot/Data", 100)]
    [Title("Loot Type")]
    [EnumToggleButtons]
    [HideLabel]
    public LootDropType lootDropType;

    
    protected override void Start()
    {
        base.Start();
        if(lootDropType == LootDropType.WorldDrop) gameObject.SetActive(!CheckIsLootedStatus());
    }

    private bool CheckIsLootedStatus()
    {
        if (WorldSaveGameManager.Instance != null)
        {
            if (!WorldSaveGameManager.Instance.currentCharacterSaveData.worldItemIsLootedStatus.ContainsKey(ID))
            {
                WorldSaveGameManager.Instance.currentCharacterSaveData.worldItemIsLootedStatus.Add(ID, false);
            }

            isLooted = WorldSaveGameManager.Instance.currentCharacterSaveData.worldItemIsLootedStatus[ID];
        }

        return isLooted;
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        // PLAY ANIMATION (TO DO)
        
        // PLAY SFX
        playerManager._controlSoundFX.PlaySFX(WorldSoundFXManager.Instance.pickItemSFX);
        
        // SET LOOTED STATUS
        if (lootDropType == LootDropType.WorldDrop)
        {
            if(WorldSaveGameManager.Instance != null)
            {  
                if (WorldSaveGameManager.Instance.currentCharacterSaveData.worldItemIsLootedStatus.ContainsKey(ID))
                {
                    WorldSaveGameManager.Instance.currentCharacterSaveData.worldItemIsLootedStatus.Remove(ID);
                }
                
                WorldSaveGameManager.Instance.currentCharacterSaveData.worldItemIsLootedStatus.Add(ID, true);
            }
        }
        
        // ADD ITEM TO INVENTORY
        switch (item.itemType)
        {
            case ItemType.Ammo:
                playerManager._controlInventory.AddAmmo(item, amount);
                break;
            case ItemType.KeyItems:
                playerManager._controlInventory.AddItem(item, amount);
                break;
        }
        
        // SHOW POPUP UI
        PlayerUIManager.Instance.popUpsManager.ShowItemPopUp(item.name, item.icon);
        
        // DESTROY OBJECT
        if(destroyObjectOnLoot) Destroy(gameObject);
    }
}
