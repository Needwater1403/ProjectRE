using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AICharacterManager : CharacterManager
{
    [TabGroup("Manager", "AI Char")] 
    public ConfigAISO AIData;
    
    [TabGroup("Manager","AI Char")]
    [Title("AI State")] 
    [SerializeField] protected AIState currentState;
    [TabGroup("Manager","AI Char")]
    [SerializeField] private AIStateHolder[] statesList;
    
    [TabGroup("Manager","AI Char")]
    [Title("Controls")] 
    public AICharacterControlMovement _controlMovement;
    [TabGroup("Manager","AI Char")]
    public AICharacterControlCombat _controlCombat;
    [TabGroup("Manager","AI Char")]
    public AICharacterControlAnimator _controlAnimator;
    [TabGroup("Manager","AI Char")]
    public AICharacterStatManager _controlStat;
    
    [TabGroup("Manager","AI Char")]
    [Title("Inventory & Equipment")]
    public AICharacterEquipmentManager _controlEquipment;
    [TabGroup("Manager","AI Char")]
    public AICharacterInventoryManager _controlInventory;
    
    [TabGroup("Manager","AI Char")]
    [Title("Nav Mesh")] 
    public NavMeshAgent navMeshAgent;

    [TabGroup("Manager", "AI Char")] 
    [Title("Drop Item")]
    public bool canDropItem;
    [TabGroup("Manager", "AI Char")] 
    public RarityRandomList<Item> dropItemList = new RarityRandomList<Item>();
    [TabGroup("Manager", "AI Char")] 
    public LootDrop_InteractableObject lootPrefab;
    
    [TabGroup("Manager", "AI Char")] 
    [Title("Flags")]
    public bool isStrafing;
    [TabGroup("Manager", "AI Char")] 
    public bool isWalkingBack;

    [HideInInspector] public Vector3 spawnPos;
    [HideInInspector] public Vector3 spawnEulerRot;
    protected override void Awake()
    {
        base.Awake();
        
        // INIT A COPY OF SCRIPTABLE OBJECT -> THE OG DONT GET MODIFIED
        _controlCombat.totalPhase = 0;
        foreach (var s in statesList)
        {
            s.state = Instantiate(s.state);
            if (s.state as CombatStanceState) _controlCombat.totalPhase++;
        }

        if (currentState == null) currentState = GetState(AIStateName.Idle);
        _controlCombat.SetCombatStanceMaxRange(this);
        SetAIDataOnStart();
    }

    protected override void Update()
    {
        base.Update();
        if(_controlCombat != null)
        {
            _controlCombat.HandleActionRecovery(this);
        }
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // if(WorldAIManager.Instance.canProcessState && !isDead)
        if(!isDead)
        {
            ProcessState();
        }
        if(_controlCombat != null)
        {
            _controlCombat.HandleStanceBreak();
        }
        
    }
    
    private void ProcessState()
    {
        var nextState = currentState?.Tick(this);
        if (nextState != null) currentState = nextState;

        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if (_controlCombat.target != null)
        {
            _controlCombat.distanceFromTarget =
                Vector3.Distance(_controlCombat.target.transform.position, transform.position);
            _controlCombat.targetDir = _controlCombat.target.transform.position - transform.position;
            _controlCombat.viewableAngle =
                WorldUltilityManager.Instance.GetAngleOfTarget(transform,
                    _controlCombat.targetDir);
        }
        
        if(isStrafing || isWalkingBack) return;
        if (navMeshAgent.enabled)
        {
            var destination = navMeshAgent.destination;
            var remainingDistance = Vector3.Distance(destination, transform.position);

            _controlAnimatorBase.SetMovingStatus(remainingDistance > navMeshAgent.stoppingDistance);
        }
        else 
        {
            _controlAnimatorBase.SetMovingStatus(false);
        }
    }
    
    public virtual void ChangePhase(AIStateName stateName)
    {
        // PLAY CHANGE PHASE ANIMATION & SFX HERE (TO DO)
        switch (stateName)
        {
            case AIStateName.CombatStancePhase2:
                _controlAnimator.PlayActionAnimationInstantly("Phase2_Start", true);
                // PLAY SFX
                break;
            case AIStateName.CombatStancePhase3:
                _controlAnimator.PlayActionAnimationInstantly("Phase3_Start", true);
                break;
        }
        
        // SET CURRENT PHASE TO NEW COMBAT STANCE PHASE (COMBAT STANCE PHASE 2)
        currentState = GetState(_controlCombat.ChangeCombatStancePhase(stateName));
        _controlCombat.SetCombatStanceMaxRange(this);
    }
    public AIState GetState(AIStateName name)
    {
        return Array.Find(statesList, stateHolder => stateHolder.name == name).state;
    }

    public virtual void OnSpawn()
    {
        
    }
    
    public override IEnumerator IEventDeath(bool manuallySelectDeathAnimation = false)
    {
        return base.IEventDeath(manuallySelectDeathAnimation);
    }
    
    protected override void DropItems()
    {
        base.DropItems();
        if (canDropItem)
        {
            // DROP ITEM BASE ON DROP RATE
            var item = dropItemList.GetRandom();
            if (item.itemType == ItemType.Empty) return;
            var loot = Instantiate(lootPrefab, transform.position + new Vector3(0,0.1f,0), Quaternion.identity);
            loot.item = item;
        }
    }

    protected virtual void ResetToSpawnPos()
    {
        transform.position = spawnPos;
        transform.eulerAngles = spawnEulerRot;
    }

    protected virtual void SetAIDataOnStart()
    {
        navMeshAgent.speed = AIData.speed;
        navMeshAgent.angularSpeed = AIData.angularSpeed;
        navMeshAgent.acceleration = AIData.acceleration;
        
        _controlAnimator.verticalMovement = AIData.verticalMovement;
        if(AIData.animatorOverrideController != null) 
            _animator.runtimeAnimatorController = AIData.animatorOverrideController;
        
        _controlCombat.SetAICombatDataOnStart(AIData);
        _controlStat.SetAIStatusDataOnStart(AIData);
    }
}
