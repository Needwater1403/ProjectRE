using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "A.I/States/Attack")]
[InlineEditor]
public class AttackState : AIState
{
    [Title("Current Attack")] 
    [HideInInspector] public AIAttackAction currentAttack;
    
    [Title("Strafe")] 
    public bool canStrafe;
    public float strafePercentage;
    public float strafePercentageAddPerTick;
    public float strafePercentageWhenReset;
    public bool canSwitchToStrafe;
    
    [Title("Flags")] 
    public bool hasPerformedAttack = false;
    public bool canPerformCombo = false;
    public bool willPerformCombo = false;

    [Title("Pivot After Attack")] 
    [SerializeField] protected bool pivotAfterAttack = false;
    public override AIState Tick(AICharacterManager aiCharacterManager)
    {
        if (aiCharacterManager._controlCombat.target == null || aiCharacterManager._controlCombat.target.isDead)
        {
            return SwitchState(aiCharacterManager, aiCharacterManager.GetState(AIStateName.Idle));
        }

        aiCharacterManager._controlCombat.RotateTowardsTargetWhilstAttacking(aiCharacterManager);
        aiCharacterManager._controlAnimatorBase.UpdateAnimationMovementParameter(0,0);
        
        if (!aiCharacterManager._controlCombat.canDoRightCombo && aiCharacterManager.isDoingAction) return this;
        // COMBO ATTACK ACTION
        if (canPerformCombo)
        {
            willPerformCombo = RollForOutcomeChance(currentAttack.chanceToDoCombo);
            PerformCombo(aiCharacterManager);
            return this;
        }
        
        // FIRST ATTACK ACTION
        if (!hasPerformedAttack)
        {
            if (aiCharacterManager._controlCombat.actionRecoveryTime > 0)
            {
                if (!canStrafe) return SwitchState(aiCharacterManager, aiCharacterManager.GetState(AIStateName.Pursue));
                var strafeState = aiCharacterManager.GetState(AIStateName.Strafe) as StrafeState;
                var temp = RollForOutcomeChance(50);
                if(temp) strafeState.willBackStep = true;
                else strafeState.willWalkBack = true;
                strafeState.isStrafingLeft = RollForOutcomeChance(50);
                return SwitchState(aiCharacterManager, strafeState); 
            }
            willPerformCombo = RollForOutcomeChance(currentAttack.chanceToDoCombo);
            PerformAttack(aiCharacterManager);
            return this;
        }
        
        // IF PERCENTAGE IS HIGH ENOUGH -> SWITCH TO STRAFE STATE
        if(canStrafe)
        {
            canSwitchToStrafe = RollForOutcomeChance(strafePercentage);
            if (canSwitchToStrafe)
            {
                strafePercentage = strafePercentageWhenReset;
                Debug.Log(" STRAFE FROM ATTACK STATE");
                var strafeState = aiCharacterManager.GetState(AIStateName.Strafe) as StrafeState;
                strafeState.SetDisengageType(RollForOutcomeChance(50));
                return SwitchState(aiCharacterManager, strafeState);
            }
            Debug.Log(" NO STRAFE");
            strafePercentage += strafePercentageAddPerTick;
        }
        return SwitchState(aiCharacterManager, aiCharacterManager.GetState(aiCharacterManager._controlCombat.currentCombatStanceName));
    }

    protected void PerformAttack(AICharacterManager aiCharacterManager)
    {
        hasPerformedAttack = true;
        aiCharacterManager._controlCombat.RotateTowardTarget(aiCharacterManager);
        currentAttack.PerformAction(aiCharacterManager);
        aiCharacterManager._controlCombat.actionRecoveryTime = currentAttack.recoveryTime;
        if (!willPerformCombo) return;
        currentAttack = currentAttack.comboAction;
        canPerformCombo = true;
    }
    
    protected void PerformCombo(AICharacterManager aiCharacterManager)
    {
        canPerformCombo = false;
        aiCharacterManager._controlCombat.canDoRightCombo = false;
        aiCharacterManager._controlCombat.RotateTowardTarget(aiCharacterManager);
        currentAttack.PerformAction(aiCharacterManager);
        aiCharacterManager._controlCombat.actionRecoveryTime = currentAttack.recoveryTime;
        if (!willPerformCombo) return;
        currentAttack = currentAttack.comboAction;
        canPerformCombo = true;
    }
    
    protected override void ResetFlags(AICharacterManager aiCharacterManager)
    {
        base.ResetFlags(aiCharacterManager);
        hasPerformedAttack = false;
        canPerformCombo = false;
        willPerformCombo = false;
        canSwitchToStrafe = false;
    }
}
