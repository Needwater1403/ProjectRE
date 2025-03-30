using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class AICharacterControlCombat : CharacterControlCombat
{
    [TabGroup("Combat","AI")]
    [SerializeField] protected AICharacterManager _aiCharacterManager;
    
    [Title("Is Undead")] 
    [HideLabel]
    [TabGroup("Combat","AI")]
    public bool isUndead = false;
    
    [Title("Line Of Sight")]
    [TabGroup("Combat","AI")]
    [SerializeField] private float lineOfSight = 13f;
    [TabGroup("Combat","AI")]
    public float minViewableAngle = -35f;
    [TabGroup("Combat","AI")]
    public float maxViewableAngle = 35f;

    [Title("Pivot")] 
    [TabGroup("Combat","AI")]
    public bool canPivot = false;
    
    [Title("Stance")] 
    [TabGroup("Combat","AI")]
    public float maxStance = 150;
    [TabGroup("Combat","AI")]
    public float currentStance;
    [TabGroup("Combat","AI")]
    [SerializeField] private float stanceRecoverySpeed = 15;
    [TabGroup("Combat","AI")]
    [SerializeField] private bool ignoreStanceBreak;
    [TabGroup("Combat","AI")]
    [SerializeField] private float stanceRecoveryTimer = 0;
    [TabGroup("Combat","AI")]
    [SerializeField] private float defaultStanceRecoveryTime = 15;
    private float stanceTickTimer = 15;
    
    [Title("Combat Stance Params")]
    [TabGroup("Combat","AI")]
    public float combatStanceMaxRange;
    [TabGroup("Combat","AI")]
    public float combatStanceMinRange;
    [TabGroup("Combat","AI")]
    public float distanceFromTarget;
    [TabGroup("Combat","AI")]
    public float viewableAngle;
    [TabGroup("Combat","AI")]
    public Vector3 targetDir;
    [TabGroup("Combat","AI")]
    public float actionRecoveryTime;
    [TabGroup("Combat","AI")]
    public float attackRotationSpeed = 25f;
    public float CombatStanceMaxRange => combatStanceMaxRange;
    public float CombatStanceMinRange => combatStanceMinRange;
    [Space]
    [TabGroup("Combat", "AI")] 
    [Title("AI Phase")]
    public AIStateName currentCombatStanceName = AIStateName.CombatStancePhase1;
    [TabGroup("Combat", "AI")] 
    public int totalPhase;
    
    protected override void Awake()
    {
        base.Awake();
    }
    public void FindTarget(AICharacterManager aiCharacterManager)
    {
        // ALREADY HAVE A TARGET
        if(target != null) return;
        
        // ATTEMPT TO FIND A NEW TARGET
        var colliders = Physics.OverlapSphere(aiCharacterManager.transform.position, lineOfSight, WorldUltilityManager.Instance.CharactersLayers);
        for (var i = 0; i < colliders.Length; i++)
        {
            var target = colliders[i].GetComponent<CharacterManager>();
            if (target != null)
            {
                // IF TARGET IS DEAD -> SKIP
                if (target.isDead) continue;

                // IF TARGET IS ITSELF -> SKIP
                if (target.transform.root == aiCharacterManager.transform.root) continue;

                // IF CAN NOT DAMAGE TARGET -> SKIP
                if (!WorldUltilityManager.CheckIfCharacterCanDamageAnother(charactersGroup, target._controlCombatBase.charactersGroup)) continue;
                
                //CHECK FIELD OF VIEW
                var targetDir = target.transform.position - aiCharacterManager.transform.position;
                var angleOfTarget = Vector3.Angle(targetDir, aiCharacterManager.transform.forward);

                if (angleOfTarget > minViewableAngle && angleOfTarget < maxViewableAngle)
                {
                    // ADD LAYER MASK FOR ENVIRONMENT LAYERS ONLY (TO DO)

                    RaycastHit hit;
                    if (Physics.Linecast(aiCharacterManager._controlCombatBase.lockOnTransform.position,
                            target._controlCombatBase.lockOnTransform.position, out hit,
                            WorldUltilityManager.Instance.EnvironmentLayers))
                    {
                        continue;
                    }
                    
                    targetDir = target.transform.position - transform.position;
                    
                    aiCharacterManager._controlCombat.SetTarget(target); 
                    if(canPivot) PivotTowardsTarget(aiCharacterManager);
                }
            }
        }
        
        // TEMP - BECAUSE PLAYER IS THE ONLY CHARACTER CAN DAMAGE THEM
        if(aiCharacterManager._controlStatsBase.currentHealth != aiCharacterManager._controlStatsBase.maxHealth)
        {
            aiCharacterManager._controlAnimator.SetCombatStatus(true);
            aiCharacterManager._controlCombat.SetTarget(PlayerManager.Instance); 
            if(canPivot) PivotTowardsTarget(aiCharacterManager);
        }
    }

    #region HANDLE

    public void HandleActionRecovery(AICharacterManager aiCharacterManager)
    {
        if (actionRecoveryTime > 0)
        {
            if (!aiCharacterManager.isDoingAction)
            {
                actionRecoveryTime -= Time.deltaTime;
            }
        }
    }
    public void HandleStanceBreak()
    {
        if(_aiCharacterManager.isDead) return;
        if (stanceRecoveryTimer > 0)
        {
            stanceRecoveryTimer -= Time.deltaTime;
        }
        else
        {
            stanceRecoveryTimer = 0;
            if (currentStance < maxStance)
            {
                stanceTickTimer += Time.deltaTime;
                if (stanceTickTimer >= 1)
                {
                    stanceTickTimer = 0;
                    currentStance += stanceRecoverySpeed;
                }
            }
            else
            {
                currentStance = maxStance;
            }
        }

        if (currentStance <= 0)
        {
            var dmgIntensity = WorldUltilityManager.Instance.GetDamageIntensityBaseOnPoiseDamage(previousPoiseDamage);
            if (dmgIntensity == DamageIntensity.Colossal)
            {
                currentStance = 1;
                return;
            }
            
            // DONT PLAY STANCE BREAK ANIMATION IF IS BREAK BECAUSE OF RIPOSTE
            currentStance = maxStance;
            if(ignoreStanceBreak) return;
            Debug.Log("STANCE BREAK");
            _aiCharacterManager._controlAnimator.PlayActionAnimationInstantly(Constants.PlayerAnimation_Stance_Break_01,true);
            //_aiCharacterManager._controlSoundFXBase.PlayStanceBreakSFX(0.25f);
        }
    }

    #endregion
    
    #region ROTATE

    public void RotateTowardTarget(AICharacterManager aiCharacterManager)
    {
        if (target == null) return;
        if (!aiCharacterManager.canRotate) return;
        if (!aiCharacterManager.isDoingAction) return;
        
        var dir = target.transform.position - aiCharacterManager.transform.position;
        dir.y = 0;
        dir.Normalize();
        if (dir == Vector3.zero) dir = aiCharacterManager.transform.forward;
        var rotation = Quaternion.LookRotation(dir);
        aiCharacterManager.transform.rotation = Quaternion.Slerp(aiCharacterManager.transform.rotation, rotation, attackRotationSpeed * Time.deltaTime);
    }
    
    public void RotateTowardAgent(AICharacterManager aiCharacterManager)
    {
        if (aiCharacterManager.isMoving)
        {
            aiCharacterManager.transform.rotation = aiCharacterManager.navMeshAgent.transform.rotation;
        }
    }
    
    public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacterManager)
    {
        if (target == null) return;
        if (!aiCharacterManager.canRotate) return;
        if (!aiCharacterManager.isDoingAction) return;

        var targetDirection = target.transform.position - aiCharacterManager.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
            targetDirection = aiCharacterManager.transform.forward;

        var targetRotation = Quaternion.LookRotation(targetDirection);
        aiCharacterManager.transform.rotation = Quaternion.Slerp(aiCharacterManager.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
    }

    #endregion

    #region PIVOT

    public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        //  PLAY A PIVOT ANIMATION DEPENDING ON VIEWABLE ANGLE OF TARGET
        if (aiCharacter.isDoingAction)
            return;
        
        if(isUndead)
        {
            switch (viewableAngle)
            {
                case >= 20 and <= 60:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Right_45", true);
                    break;
                case <= -20 and >= -60:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Left_45", true);
                    break;
                case >= 61 and <= 110:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Right_90", true);
                    break;
                case <= -61 and >= -110:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Left_90", true);
                    break;
                case >= 111 and <= 145:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Right_135", true);
                    break;
                case <= -111 and >= -145:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Left_135", true);
                    break;
                case >= 146 and <= 180:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Right_180", true);
                    break;
                case <= -146 and >= -180:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Left_180", true);
                    break;
            }
        }
        else
        {
            switch (viewableAngle)
            {
                case >= 45 and <= 120:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Right_90", true);
                    break;
                case <= -45 and >= -120:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Left_90", true);
                    break;
                case >= 121 and <= 180:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Right_180", true);
                    break;
                case <= -121 and >= -180:
                    aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Left_180", true);
                    break;
            }
        }
    }

    #endregion

    #region STANCE

    public void SetCombatStanceMaxRange(AICharacterManager aiCharacterManager)
    {
        var combatStanceState = aiCharacterManager.GetState(currentCombatStanceName) as CombatStanceState;
        if (combatStanceState != null)
        {
            combatStanceMaxRange = combatStanceState.maxAggroDistance;
            combatStanceMinRange = combatStanceState.minAggroDistance;
        }
    }

    public AIStateName ChangeCombatStancePhase(AIStateName name)
    {
        currentCombatStanceName = name;
        return currentCombatStanceName;
    }
    
    public void TakeStanceDamage(int stanceDamage)
    {
        stanceRecoveryTimer = defaultStanceRecoveryTime;
        currentStance -= stanceDamage;
    }

    #endregion

    #region COMBO

    public override void EnableCombo()
    {
        canDoRightCombo = true;
    }
    
    public override void DisableCombo()
    {
        canDoRightCombo = false;
    }

    #endregion

    public virtual void SetAICombatDataOnStart(ConfigAISO data)
    {
        lineOfSight = data.lineOfSight;
        minViewableAngle = data.minViewableAngle;
        maxViewableAngle = data.maxViewableAngle;
        canPivot = data.canPivot;
        maxStance = data.maxStance;
        currentStance = maxStance;
        stanceRecoverySpeed = data.stanceRecoverySpeed;
        ignoreStanceBreak = data.ignoreStanceBreak;
        defaultStanceRecoveryTime = data.defaultStanceRecoveryTime;
        attackRotationSpeed = data.attackRotationSpeed;
        totalPhase = data.totalPhase;
        SetAIDamageDataOnStart(data);
    }
    
    protected virtual void SetAIDamageDataOnStart(ConfigAISO data)
    {
        
    }
    
}
