using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHPBar : UI_StatBar
{
    [Title("HP Owner")] 
    [SerializeField] private AIBossCharacterManager hpOwnerManager;
    public AIBossCharacterManager HpOwnerManager => hpOwnerManager;
    public void EnableBossHPBar(AIBossCharacterManager aiBossCharacterManager)
    {
        hpOwnerManager = aiBossCharacterManager;
        SetMaxStat(hpOwnerManager._controlStatsBase.currentHealth);
    }

    public override void SetMaxStat(float maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = maxValue;
    }

    public override void SetStat(float newValue)
    {
        _slider.value = newValue;
    }
    
    public AIBossCharacterManager GetBossManager()
    {
        return hpOwnerManager;
    }
}