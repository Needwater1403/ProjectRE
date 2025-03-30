using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager Instance;
    [SerializeField] private List<InteractableObjectSpawner> interactableObjectSpawnerPrefabsList;
    [SerializeField] private List<InteractableObject> interactableObjectList;
    
    [Title("Flags")] 
    public bool isCompletelySpawned;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    private void Start()
    {
        // SPAWN AI CHARACTERS AFTER LOAD SCENE
        AttemptToSpawnInteractableObjectsAfterLoadScene();
    }

    public void AttemptToSpawnInteractableObjectsAfterLoadScene()
    {
        StartCoroutine(SpawnInteractableObjectsAfterLoadScene());
    }
    private IEnumerator SpawnInteractableObjectsAfterLoadScene()
    {
        // WAIT FOR THE ENVIRONMENT SCENE TO LOAD
        yield return new WaitForSeconds(1);
        // while (!SceneManager.GetActiveScene().isLoaded)
        // {
        //     yield return null;
        // }
        
        // SPAWN AI CHARACTERS
        SpawnObjects();
    }
    
    private void SpawnObjects()
    {
        foreach (var acm in interactableObjectSpawnerPrefabsList)
        {
            acm.Spawn(interactableObjectList);
        }

        StartCoroutine(CheckSpawnObjectsProgress());
    }
    
    private IEnumerator CheckSpawnObjectsProgress()
    {
        while (interactableObjectList.Count(io => io.isCompletelySpawned) < interactableObjectSpawnerPrefabsList.Count())
        {
            Debug.Log($"Items loaded: {interactableObjectList.Count(io => io.isCompletelySpawned)}/{interactableObjectSpawnerPrefabsList.Count()}");
            yield return new WaitForEndOfFrame();
        }

        yield return null;
        isCompletelySpawned = true;
    }
    
    
    public void AddObjectSpawner(InteractableObjectSpawner spawner)
    {
        interactableObjectSpawnerPrefabsList.Add(spawner);
    }
    
    public void RemoveObjectSpawner()
    {
        foreach (var spawner in interactableObjectSpawnerPrefabsList.ToList().Where(ob => ob == null))
        {
            interactableObjectSpawnerPrefabsList.Remove(spawner);
        }
        
        foreach (var ai in interactableObjectList.ToList().Where(ob => ob == null))
        {
            interactableObjectList.Remove(ai);
        }
        
    }

    public void DestroyAllObjects()
    {
        foreach (var acm in interactableObjectList)
        {
            Destroy(acm.gameObject);
        }
    }
    
}
