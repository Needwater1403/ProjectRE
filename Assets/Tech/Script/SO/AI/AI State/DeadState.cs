using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Dead")]
public class DeadState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacterManager)
    {

        return base.Tick(aiCharacterManager);
    }
}
