using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AICharacterStatManager : CharacterStatManager
{
    [TabGroup("Stat", "AI Char")] 
    [SerializeField] private AICharacterManager _aiCharacterManager;

    [TabGroup("Stat", "AI Char")] 
    [Title("Change Phase Params")] 
    public bool changePhaseBasedOnHP;
    [TabGroup("Stat", "AI Char")]
    [ShowIf("ShowPhase2Cap")][Range(0,100)] [SuffixLabel("%")]
    public int phase2CapPercentage;
    [TabGroup("Stat", "AI Char")] 
    [ShowIf("ShowPhase3Cap")] [Range(0,100)] [SuffixLabel("%")]
    public int phase3CapPercentage;
    
    private bool hasChangedToPhase2;
    private bool hasChangedToPhase3;
    private int phase2CapHP;
    private int phase3CapHP;
    protected override void Start()
    {
        base.Start();
        phase2CapHP = Mathf.RoundToInt(maxHealth * phase2CapPercentage / 100);
        phase3CapHP = Mathf.RoundToInt(maxHealth * phase3CapPercentage / 100);
    }

    #region SHOW IF FUNC

    private bool ShowPhase2Cap()
    {
        if (_aiCharacterManager._controlCombat.totalPhase >= 2 && changePhaseBasedOnHP)
        {
            hasChangedToPhase2 = false;
            return true;
        }
        hasChangedToPhase2 = true;
        return false;
    }
    
    private bool ShowPhase3Cap()
    {
        if (_aiCharacterManager._controlCombat.totalPhase >= 3 && changePhaseBasedOnHP)
        {
            hasChangedToPhase3 = false;
            return true;
        }
        hasChangedToPhase3 = true;
        return false;
    }

    #endregion
    public override void CheckHealth()
    {
        base.CheckHealth();
        if (!hasChangedToPhase2 && currentHealth <= phase2CapHP)
        {
            hasChangedToPhase2 = true;
            if(currentHealth <= 0) return;
            _aiCharacterManager.ChangePhase(AIStateName.CombatStancePhase2);
            Debug.Log("PHASE 2");
        }
        
        if (!hasChangedToPhase3 && currentHealth <= phase3CapHP)
        {
            hasChangedToPhase3 = true;
            if(currentHealth <= 0) return;
            _aiCharacterManager.ChangePhase(AIStateName.CombatStancePhase3);
            Debug.Log("PHASE 3");
        }
    }
    
    public override void SetStatWhenStartGame()
    {
        base.SetStatWhenStartGame();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }
    
    public virtual void SetAIStatusDataOnStart(ConfigAISO data)
    {
        maxHealth = data.maxHealth;
        maxStamina = data.maxStamina;
        poiseDefense = data.poise;
        defaultPoiseResetTimer = data.defaultPoiseResetTimer;
        changePhaseBasedOnHP = data.changePhaseBasedOnHp;
        
        phase2CapPercentage = data.phase2CapPercentage;
        phase3CapPercentage = data.phase3CapPercentage;
    }
}
