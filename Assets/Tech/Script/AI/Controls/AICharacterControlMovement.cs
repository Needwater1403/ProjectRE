using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterControlMovement : CharacterControlMovement
{
    public void RotateTowardAgent(AICharacterManager aiCharacterManager)
    {
        if(aiCharacterManager.isMoving) aiCharacterManager.transform.rotation = aiCharacterManager.navMeshAgent.transform.rotation;
    }
}
