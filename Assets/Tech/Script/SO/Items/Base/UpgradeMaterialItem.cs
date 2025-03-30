using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UpgradeMaterialItem : Item
{
    [TabGroup("Item","Material")]
    public string materialID;
    [TabGroup("Item","Material")]
    [TextArea] public string effectDescription;
    public virtual void SetUpID()
    {
        
    }
    
    public virtual void ConsumeItem(PlayerManager playerManager, int consumeAmount)
    {
        if(amount - consumeAmount < 0) return;
        amount -= consumeAmount;
        if (amount <= 0)
        {
            amount = 0;
            // REMOVE ITEM FROM INVENTORY
            Debug.Log("REMOVE");
            //playerManager._controlInventory.RemoveItem(this);
        }
    }
}
