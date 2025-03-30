using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Pursue")]
[InlineEditor]
public class PursueState : AIState
{
    public float stoppingDistance;
    private bool gotStoppingDistance;

    public override AIState Tick(AICharacterManager aiCharacterManager)
    {
        // IF IS DOING ANY ACTION -> WAIT FOR THE ACTION TO FINISH
        if (aiCharacterManager.isDoingAction) return this;
        
        // GET STOPPING DISTANCE
        if (!gotStoppingDistance)
        {
            stoppingDistance = UnityEngine.Random.Range(aiCharacterManager._controlCombat.combatStanceMaxRange, aiCharacterManager._controlCombat.combatStanceMinRange);
        }
        
        if (!aiCharacterManager.isCombat)
        {
            // TEMP BECAUSE I ONLY HAVE ALERT ANIMATION FOR UNDEAD
            if(aiCharacterManager._controlCombat.isUndead) aiCharacterManager._controlAnimator.PlayActionAnimation("Alert_01", true);
            aiCharacterManager._controlAnimatorBase.SetCombatStatus(true);
        }
        
        // IF NO TARGET -> SWITCH BACK TO IDLE
        if (aiCharacterManager._controlCombat.target == null || aiCharacterManager._controlCombat.target.isDead)
        {
            return aiCharacterManager.GetState(AIStateName.Idle);
        }
        
        // ENABLE NAVMESH IF IS NOT ACTIVE
        if (!aiCharacterManager.navMeshAgent.enabled) aiCharacterManager.navMeshAgent.enabled = true;
        if (!aiCharacterManager._animator.applyRootMotion) aiCharacterManager._animator.applyRootMotion = true;
        
        // TURN
        if (!aiCharacterManager.isMoving && aiCharacterManager._controlCombat.canPivot)
        {
            if (aiCharacterManager._controlCombat.viewableAngle < aiCharacterManager._controlCombat.minViewableAngle 
                || aiCharacterManager._controlCombat.viewableAngle > aiCharacterManager._controlCombat.maxViewableAngle)
            {
                // PIVOT
                aiCharacterManager._controlCombat.PivotTowardsTarget(aiCharacterManager);
            }
        }
        
        aiCharacterManager._controlMovement.RotateTowardAgent(aiCharacterManager);
        
        if (aiCharacterManager._controlCombat.viewableAngle > aiCharacterManager._controlCombat.minViewableAngle 
            && aiCharacterManager._controlCombat.viewableAngle < aiCharacterManager._controlCombat.maxViewableAngle)
        {
            aiCharacterManager.navMeshAgent.stoppingDistance = stoppingDistance;
        }
        else
        {
            aiCharacterManager.navMeshAgent.stoppingDistance = 1;
        }
        // IF TARGET IS OUT OF REACH => RETURN TO ORIGINAL POS (TO DO)
        
        // IF WITHIN COMBAT RANGE => SWITCH TO COMBAT STANCE STATE
        if (aiCharacterManager._controlCombat.distanceFromTarget <= aiCharacterManager.navMeshAgent.stoppingDistance) // TEMP
        {
            return SwitchState(aiCharacterManager, aiCharacterManager.GetState(aiCharacterManager._controlCombat.currentCombatStanceName));
            
        }
            
        // PURSUE TARGET
        var path = new NavMeshPath();
        aiCharacterManager.navMeshAgent.CalculatePath(aiCharacterManager._controlCombat.target.transform.position,
            path);
        aiCharacterManager.navMeshAgent.SetPath(path);
        return this;
    }
}
