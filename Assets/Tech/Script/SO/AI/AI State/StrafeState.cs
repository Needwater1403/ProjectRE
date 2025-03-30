using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(menuName = "A.I/States/Strafe")]
[InlineEditor]
public class StrafeState : AIState
{
    public bool willBackStep;
    public bool willWalkBack;
    private bool endDisengagePhase;
    public float strafeTime;
    public float walkBackTime;
    public float timer;
    public bool isStrafingLeft;
    
    public override AIState Tick(AICharacterManager aiCharacterManager)
    {
        // DISENGAGE PHASE
        if(willBackStep)
        {
            willBackStep = false;
            aiCharacterManager._controlAnimatorBase.PlayActionAnimation("BackStep_01", true);
        }
        else switch (willWalkBack)
        {
            case true:
                willWalkBack = false;
                aiCharacterManager.isDoingAction = true;
                break;
            case false when !endDisengagePhase:
            {
                timer += Time.deltaTime;
                if (timer >= walkBackTime)
                {   
                    timer = 0;
                    aiCharacterManager.isDoingAction = false;
                    aiCharacterManager._controlAnimator.SetWalkBackStatus(false);
                    endDisengagePhase = true;
                }
                else
                {
                    if (!aiCharacterManager._animator.applyRootMotion)
                        aiCharacterManager._animator.applyRootMotion = true;
                    
                    // TURN
                    aiCharacterManager._controlMovement.RotateTowardAgent(aiCharacterManager);
                    aiCharacterManager.transform.LookAt(aiCharacterManager._controlCombat.target.transform.position);
                    aiCharacterManager._controlAnimator.SetWalkBackStatus(true);
                }
                break;
            }
        }
        if (aiCharacterManager.isDoingAction) return this;
        
        // STRAFING PHASE
        timer += Time.deltaTime;
        if (timer >= strafeTime)
        {
            Debug.Log("PURSUE FROM STRAFE");
            // IF WITHIN COMBAT RANGE => SWITCH TO COMBAT STANCE STATE
            if (aiCharacterManager._controlCombat.distanceFromTarget <= aiCharacterManager.navMeshAgent.stoppingDistance) // TEMP
            {
                return SwitchState(aiCharacterManager, aiCharacterManager.GetState(aiCharacterManager._controlCombat.currentCombatStanceName));
            }
            return SwitchState(aiCharacterManager, aiCharacterManager.GetState(AIStateName.Pursue));
        }
        
        // DISABLE NAVMESH
        // if (aiCharacterManager.navMeshAgent.enabled) aiCharacterManager.navMeshAgent.enabled = false;
        if (!aiCharacterManager._animator.applyRootMotion) aiCharacterManager._animator.applyRootMotion = true;
        
        // TURN
        aiCharacterManager._controlMovement.RotateTowardAgent(aiCharacterManager);
        aiCharacterManager.transform.LookAt(aiCharacterManager._controlCombat.target.transform.position);
        
        
        aiCharacterManager._controlAnimator.SetStrafingStatus(true, isStrafingLeft);
        
        return this;
    }

    public void SetDisengageType(bool _willBackStep)
    {
        // if (_willBackStep) willBackStep = true;
        // else willWalkBack = true;
        willWalkBack = true;
        strafeTime = Random.Range(1.5f, 2.5f);
        walkBackTime = Random.Range(1f, 1.8f);
    }
    protected override void ResetFlags(AICharacterManager aiCharacterManager)
    {
        base.ResetFlags(aiCharacterManager);
        aiCharacterManager._controlAnimator.SetStrafingStatus(false);
        timer = 0;
        willBackStep = false;
        willWalkBack = false;
        endDisengagePhase = false;
    }
}