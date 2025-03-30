using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "A.I/States/Combat Stance")]
[InlineEditor]
public class CombatStanceState : AIState
{
    [Title("Attack Action")]
    public List<AIAttackAction> allAIAttackActions;                     // ALL ATTACK ACTIONS FOR THIS AI CHAR
    public RarityRandomList<AIAttackAction> availableAIAttackActions;    // ALL POSSIBLE ATTACK ACTIONS FOR THIS AI CHAR UNDER CERTAIN CIRCUMSTANCES 
    public AIAttackAction selectedAttackAction;
    private AIAttackAction previousAttackAction;
    protected bool hasAttackActionReady = false;
    
    [Title("Combo")] 
    [SerializeField] protected bool canDoCombo = false;
    [Range(0.0f, 100.0f)]
    [SerializeField] protected float chanceToDoCombo = 25;
    private bool hasRolledForComboChance = false;
    private int getNewAttackAttemptCount = 0;
    
    [Title("Aggro Distance")] 
    public float maxAggroDistance = 2;
    public float minAggroDistance = 0;
    
    [Title("Strafe")] 
    public bool canStrafe;

    public override AIState Tick(AICharacterManager aiCharacterManager)
    {
        if (aiCharacterManager.isDoingAction) return this;
        if (!aiCharacterManager.navMeshAgent.enabled) aiCharacterManager.navMeshAgent.enabled = true;
        if (!aiCharacterManager.isCombat) aiCharacterManager._controlAnimatorBase.SetCombatStatus(true);
        
        // IF OUT OF RANGE -> SWITCH TO PURSUE STATE
        if (aiCharacterManager._controlCombat.distanceFromTarget > aiCharacterManager._controlCombat.CombatStanceMaxRange
            || getNewAttackAttemptCount == 3)
        {
            getNewAttackAttemptCount = 0;
            // IF PERCENTAGE IS HIGH ENOUGH -> SWITCH TO STRAFE STATE
            if (!canStrafe) return SwitchState(aiCharacterManager, aiCharacterManager.GetState(AIStateName.Pursue));
            var strafeState = aiCharacterManager.GetState(AIStateName.Strafe) as StrafeState;
            strafeState.SetDisengageType(RollForOutcomeChance(50));
            strafeState.isStrafingLeft = RollForOutcomeChance(50);
            return SwitchState(aiCharacterManager, strafeState);
        }
       
        // IF TARGET IS DEAD -> SWITCH TO IDLE STATE
        if (aiCharacterManager._controlCombat.target == null || aiCharacterManager._controlCombat.target.isDead)
        {
            return SwitchState(aiCharacterManager, aiCharacterManager.GetState(AIStateName.Idle));
        }
        
        // TURN
        if (!aiCharacterManager.isMoving && aiCharacterManager._controlCombat.canPivot)
        {
            if (aiCharacterManager._controlCombat.viewableAngle is < -35 or > 35)
            {
                // PIVOT
                aiCharacterManager._controlCombat.PivotTowardsTarget(aiCharacterManager);
            }
        }
        
        // ROTATE 
        aiCharacterManager._controlCombat.RotateTowardAgent(aiCharacterManager);
        
        // GET ATTACK
        if(!hasAttackActionReady)
        {
            Debug.Log("GET NEW ATTACK");
            GetNextAttack(aiCharacterManager);
        }
        else
        {
            // PASS ATTACK ACTION AND SWITCH TO ATTACK STATE
            var attackState = aiCharacterManager.GetState(AIStateName.Attack) as AttackState;
            if (attackState != null) attackState.currentAttack = selectedAttackAction;
            Debug.Log("Switch to ATTACK STATE");
            return SwitchState(aiCharacterManager, attackState);
        }

        return this;
    }

    protected virtual void GetNextAttack(AICharacterManager aiCharacterManager)
    {
        availableAIAttackActions = new RarityRandomList<AIAttackAction>();
        
        // SORT THROUGH ALL ATTACK ACTIONS TO FIND DOABLE ACTIONS AND PUT THEM INTO A SECOND LIST
        foreach (var atkAction in allAIAttackActions)
        {
            if (atkAction.minAttackDistance > aiCharacterManager._controlCombat.distanceFromTarget) continue;
            if (atkAction.maxAttackDistance < aiCharacterManager._controlCombat.distanceFromTarget) continue;
            if (atkAction.minAttackAngle > aiCharacterManager._controlCombat.viewableAngle) continue;
            if (atkAction.maxAttackAngle < aiCharacterManager._controlCombat.viewableAngle) continue;
            availableAIAttackActions.Add(atkAction, atkAction.weight);
        }
        
        if(availableAIAttackActions.Count == 0)
        {
            getNewAttackAttemptCount++;
            return;
        }
        getNewAttackAttemptCount = 0;
        
        // SELECT RANDOMLY FROM THE SECOND LIST AND PASS THIS ACTION TO ATTACK STATE 
        selectedAttackAction = availableAIAttackActions.GetRandom();
        previousAttackAction = selectedAttackAction;
        hasAttackActionReady = true;
    }   
    
    protected override void ResetFlags(AICharacterManager aiCharacterManager)
    {
        base.ResetFlags(aiCharacterManager);
        hasRolledForComboChance = false;
        hasAttackActionReady = false;
    }
}
