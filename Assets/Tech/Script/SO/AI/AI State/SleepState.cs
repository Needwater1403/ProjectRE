using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "A.I/States/Sleep")]
public class SleepState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacterManager)
    {
        return this;
    }
}
