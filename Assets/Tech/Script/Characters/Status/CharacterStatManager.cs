using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterStatManager : MonoBehaviour
{
    protected CharacterManager _characterManager;
    [TabGroup("Stat","Attribute")]
    [Title("Current Value")] 
    public float currentHealth; 
    [TabGroup("Stat","Attribute")]
    public float currentStamina;
    
    [Title("Max Value")]
    [OnValueChanged("OnMaxHealthChange",true)]
    [TabGroup("Stat","Attribute")]
    public float maxHealth;
    [OnValueChanged("OnMaxStaminaChange",true)]
    [TabGroup("Stat","Attribute")]
    public float maxStamina;
    
    private float staminaRegenTimer;
    private float staminaTickTimer;
    [HideInInspector] public ConfigStaminaConsumptionSO staminaConsumptionS0;
    
    [TabGroup("Stat", "Base")] 
    [Title("Poise")]
    public float totalPoiseDmgTaken;
    [TabGroup("Stat", "Base")] 
    public float poiseBonus;
    [TabGroup("Stat", "Base")] 
    public float poiseDefense;
    [TabGroup("Stat", "Base")] 
    public float defaultPoiseResetTimer;
    [TabGroup("Stat", "Base")] 
    public float poiseResetTimer;
    
    [Title("Others")]
    [TabGroup("Stat", "Base")] 
    public float damageTakenPercentage;
    
    protected virtual void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {
        if(ConfigSOManager.Instance == null) return;
        staminaConsumptionS0 = ConfigSOManager.Instance.PlayerStaminaConsumptionConfigSO;
        SetStatWhenStartGame();
    }

    protected virtual void Update()
    {
        HandlePoiseReset();
    }

    public virtual void SetStatWhenStartGame()
    {

    }
    #region STAMINA

    public void StaminaRegen()
    {
        if(_characterManager.canSprint || _characterManager.isDoingAction) return;
        if (currentStamina >= maxStamina) return;
        staminaRegenTimer += Time.deltaTime;
        if (staminaRegenTimer >= staminaConsumptionS0.staminaRegenDelayTime)
        {
            staminaTickTimer += Time.deltaTime;
            if (staminaTickTimer >= .1f)
            {
                staminaTickTimer = 0;
                SetStamina(staminaConsumptionS0.staminaRegenAmount);
            }
        }
    }
    
    public void ResetStaminaRegenTimer()
    {
        staminaRegenTimer = 0;
    }
    
    public virtual void SetStamina(float addValue)
    {
        var previousStaminaValue =  currentStamina;
        currentStamina += addValue;
        if (currentStamina > maxStamina) currentStamina = maxStamina;
        // UPDATE UI IF THE CHARACTER IS PLAYER
        if(gameObject.CompareTag(Constants.PlayerTag))
        {
            PlayerUIManager.Instance.hubManager.SetNewStaminaValue(previousStaminaValue, currentStamina, maxStamina);
        }
    }

    protected virtual void OnMaxStaminaChange()
    {
        
    }
    #endregion

    #region HEALTH

    public virtual void SetHealth(float addValue)
    {
        var oldHealthValue =  currentHealth;
        currentHealth += addValue;

        // CHECK IF HEALTH < 0 OR > MAX VALUE
        CheckHealth();
        
        // UPDATE UI IF THE CHARACTER IS PLAYER
        if(gameObject.CompareTag(Constants.PlayerTag))
        {
            PlayerUIManager.Instance.hubManager.SetNewHealthValue(currentHealth/ maxHealth);
        }
        else if(gameObject.CompareTag(Constants.BossTag))
        {
            PlayerUIManager.Instance.hubManager.SetBossNewHealthValue(oldHealthValue, currentHealth);
        }
        
    }
    
    public virtual void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            StartCoroutine(_characterManager.IEventDeath(_characterManager._controlCombatBase.isBeingRiposteOrBackStab));
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    
    protected virtual void OnMaxHealthChange()
    {

    }
    #endregion
    
    public virtual void HandlePoiseReset()
    {
        if (poiseResetTimer > 0) poiseResetTimer -= Time.deltaTime;
        else totalPoiseDmgTaken = 0;
    }
}
