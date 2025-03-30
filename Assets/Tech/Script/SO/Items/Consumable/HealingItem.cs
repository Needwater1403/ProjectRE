using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Items/Consumables/Healing Item")]
[InlineEditor]
public class HealingItem : ConsumableItem
{
    [TabGroup("Item", "Healing Item")] 
    public bool healBaseOnMaxHPPercentage;
    [TabGroup("Item", "Healing Item")] 
    public int healAmount;
    
    public override void PlayAnimation(PlayerManager playerManager)
    {
        base.PlayAnimation(playerManager);
        animationName = Constants.PlayerAnimation_Healing_01;
        playerManager._controlAnimator.PlayActionAnimation(animationName, 
            true, false, true, true);
    }

    public override void PlayCantUseAnimation(PlayerManager playerManager)
    {
        base.PlayAnimation(playerManager);
        animationName = Constants.PlayerAnimation_Healing_Empty_01;
        playerManager._controlAnimator.PlayActionAnimation(animationName, 
            true, false, true, true);
    }
    
    public override bool CheckIsUsable(PlayerManager playerManager)
    {
        return amount > 0;
    }
    
    public override void PerformItemAction(PlayerManager playerManager)
    {
        if(amount<=0) return;
        ConsumeItem(playerManager);
        playerManager._controlStatsBase.SetHealth(healBaseOnMaxHPPercentage?playerManager._controlStatsBase.maxHealth*healAmount/100:healAmount);
        PlayerUIManager.Instance.hubManager.SetPillsText(amount);
    }
}

