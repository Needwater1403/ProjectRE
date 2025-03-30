using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerStatManager : CharacterStatManager
{
    [SerializeField] private PlayerManager _playerManager;
    protected ConfigStatsSO _configStatsSO;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void SetStatWhenStartGame()
    {
        _configStatsSO = ConfigSOManager.Instance.PlayerStatConfigSO;
        maxStamina = _configStatsSO.maxStamina;
        maxHealth = _configStatsSO.maxHealth;
        
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        
        if(PlayerUIManager.Instance!=null) PlayerUIManager.Instance.SetPlayerUIOnStart();
    }
    
}
