using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Back Step")]
[InlineEditor]
public class BackStepState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacterManager)
    {
        aiCharacterManager._controlAnimatorBase.PlayActionAnimation("BackStep_01", true);
        return SwitchState(aiCharacterManager, aiCharacterManager.GetState(AIStateName.Strafe));
    }
    
    protected override void ResetFlags(AICharacterManager aiCharacterManager)
    {
        base.ResetFlags(aiCharacterManager);
    }
}
