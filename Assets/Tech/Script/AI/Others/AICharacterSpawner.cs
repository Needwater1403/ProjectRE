using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [SerializeField] private RarityRandomList<GameObject> characterPrefabList;
    [SerializeField] private RarityRandomList<Transform> spawnPoint;
    private List<Transform> chosenSpawnPoint = new List<Transform>();
    private List<Transform> allSpawnPoint = new List<Transform>();
    private GameObject character;
    [Space]
    public int amount;
    [HideInInspector] public bool isCompletelySpawned;
   
    public void Start()
    {
        WorldAIManager.Instance.AddAISpawner(this);
        for (var i = 0; i < amount; i++)
        {
            var point = spawnPoint.GetRandom();
            while(chosenSpawnPoint.Contains(point))
            {
                point = spawnPoint.GetRandom();
            }
            chosenSpawnPoint.Add(point);
        }
        
        for (var i = 0; i < spawnPoint.Count; i++)
        {
            var point = spawnPoint.GetRandom();
            while(allSpawnPoint.Contains(point))
            {
                point = spawnPoint.GetRandom();
            }
            allSpawnPoint.Add(point);
        }
    }
    public void Spawn(List<AICharacterManager> list)
    {
        if (!characterPrefabList.IsUnityNull())
        {
            foreach (var point in chosenSpawnPoint)
            {
                character = Instantiate(characterPrefabList.GetRandom(), point.position, point.rotation, point.parent);
                var aiManager = character.GetComponent<AICharacterManager>();
                aiManager.OnSpawn();
                list.Add(aiManager);
                aiManager.spawnPos = character.transform.position;
                aiManager.spawnEulerRot = character.transform.eulerAngles;
            }

            foreach (var point in allSpawnPoint)
            {
                point.gameObject.SetActive(false);
            }
        }
    }
}
