using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager Instance;
    [SerializeField] private List<AICharacterSpawner> AICharactersSpawnerPrefabsList;
    [SerializeField] private List<AICharacterManager> AICharactersList;
    
    [Title("Holders")] 
    [SerializeField] private GameObject basement;
    [SerializeField] private GameObject firstFloor;
    [SerializeField] private GameObject secondFloor;
    [SerializeField] private GameObject outside;
    
    [Title("Flags")] 
    public bool isCompletelySpawned;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Title("AI Behavior Toggle")] 
    public bool canProcessState = false;
    
    private void Start()
    {
        // SPAWN AI CHARACTERS AFTER LOAD SCENE
        AttemptToSpawnAICharactersAfterLoadScene();
    }

    public void AttemptToSpawnAICharactersAfterLoadScene()
    {
        StartCoroutine(SpawnAICharactersAfterLoadScene());
    }
    private IEnumerator SpawnAICharactersAfterLoadScene()
    {
        yield return new WaitForSeconds(.01f);
        // SPAWN AI CHARACTERS
        SpawnAICharacters();
    }

    private void SpawnAICharacters()
    {
        foreach (var acm in AICharactersSpawnerPrefabsList)
        {
            acm.Spawn(AICharactersList);
        }

        firstFloor.gameObject.SetActive(false);
        secondFloor.gameObject.SetActive(false);
        outside.gameObject.SetActive(false);
        StartCoroutine(CheckSpawnObjectsProgress());
    }

    private IEnumerator CheckSpawnObjectsProgress()
    {
        while (AICharactersSpawnerPrefabsList.Count(io => io.isCompletelySpawned) != AICharactersSpawnerPrefabsList.Count())
        {
            yield return new WaitForEndOfFrame();
        }

        yield return null;
        isCompletelySpawned = true;
    }
    
    public void AddAISpawner(AICharacterSpawner spawner)
    {
        AICharactersSpawnerPrefabsList.Add(spawner);
    }
    
    public void RemoveAISpawner()
    {
        foreach (var spawner in AICharactersSpawnerPrefabsList.ToList().Where(ob => ob == null))
        {
            AICharactersSpawnerPrefabsList.Remove(spawner);
        }
        
        foreach (var ai in AICharactersList.ToList().Where(ob => ob == null))
        {
            AICharactersList.Remove(ai);
        }
    }
    
    [Button("Destroy")]
    public void DestroyAICharacters()
    {
        foreach (var acm in AICharactersList)
        {
            Destroy(acm.gameObject);
        }
    }
    
    private void DisableAICharacters()
    {
        foreach (var acm in AICharactersList)
        {
            // var bossManager = acm as AIBossCharacterManager;
            // if (bossManager != null)
            // {
            //     foreach (var cam in bossManager.cinematicCams)
            //     {
            //         Destroy(cam);
            //     }
            // }
            Destroy(acm.gameObject);
        }
        AICharactersList.Clear();
    }

    public void ResetAllAI()
    {
        DisableAICharacters();
        SpawnAICharacters();
    }
}
