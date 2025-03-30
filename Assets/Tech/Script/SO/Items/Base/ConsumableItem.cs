using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ConsumableItem : Item
{
    protected string animationName;
    [TabGroup("Item", "Consumable")] 
    [TextArea] public string effectDescription;
    public virtual void PlayAnimation(PlayerManager playerManager)
    {
        
    }
    public virtual void PlayCantUseAnimation(PlayerManager playerManager)
    {
        
    }
    public virtual void PerformItemAction(PlayerManager playerManager)
    {

    }
    public virtual bool CheckIsUsable(PlayerManager playerManager)
    {
        return true;
    }
    
    public virtual void ConsumeItem(PlayerManager playerManager)
    {
        amount--;
        // COMMENT DUE TO DEMO => WILL BE UNCOMMENTED LATER
        
        // if (amount <= 0)
        // {
        //     amount = 0;
        //     // REMOVE ITEM FROM INVENTORY
        //     Debug.Log("REMOVE " + ID);
        //     playerManager._controlInventory.RemoveItem(this);
        //     
        //     for (var i = 0; i < playerManager._controlInventory.consumableItemQuickSlots.Length; i++)
        //     {
        //         if (playerManager._controlInventory.consumableItemQuickSlots[i] == this) playerManager._controlInventory.consumableItemQuickSlots[i] = null;
        //     }
        // }
    }
    
}
