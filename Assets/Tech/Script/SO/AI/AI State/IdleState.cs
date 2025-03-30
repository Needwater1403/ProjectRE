using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Idle")]
[InlineEditor]
public class IdleState : AIState
{
    public bool returnToIdleAnimPreCombat;
    public override AIState Tick(AICharacterManager aiCharacterManager)
    {
        // UNCOMMENT MAKE THE AI GO BACK TO THE IDLE ANIMATION (PRE COMBAT)
        if (returnToIdleAnimPreCombat && aiCharacterManager.isCombat) aiCharacterManager._controlAnimatorBase.SetCombatStatus(false);
        
        // SWITCH TO PURSUE
        // if (aiCharacterManager._controlCombatBase.target != null && !aiCharacterManager._controlCombatBase.target.isDead)
        // {
        //     return SwitchState(aiCharacterManager, aiCharacterManager.GetState(AIStateName.Pursue));
        // }
        
        // STAY IDLE
        aiCharacterManager._controlCombat.FindTarget(aiCharacterManager);
        return this;
    }
}
