using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AICharacterSpawnerHolder : MonoBehaviour
{
    public List<AICharacterSpawner> _aiCharacterSpawners = new List<AICharacterSpawner>();
    
    [Button]
    public void Spawn()
    {
        foreach (var spawner in _aiCharacterSpawners)
        {
            //spawner.HandleSpawnAI();
        }
    }
}
