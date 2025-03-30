using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private SceneField[] sceneToLoad;
    [SerializeField] private SceneField[] sceneToUnload;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PlayerTag))
        {
            LoadScenes();
            UnloadScenes();
        }
    }

    private void LoadScenes()
    {
        for (int i = 0; i < sceneToLoad.Length; i++)
        {
            bool isSceneLoaded = false;
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                var loadedScene = SceneManager.GetSceneAt(j);
                if (loadedScene.name == sceneToLoad[i].SceneName)
                {
                    isSceneLoaded = true;
                    break;
                }
            }

            if (!isSceneLoaded)
            {
                SceneManager
                    .LoadSceneAsync(sceneToLoad[i], LoadSceneMode.Additive);
            }
        }
    }

    private void UnloadScenes()
    {
        for (int i = 0; i < sceneToLoad.Length; i++)
        {
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                var loadedScene = SceneManager.GetSceneAt(j);
                if (loadedScene.name == sceneToLoad[i].SceneName)
                {
                    SceneManager
                        .UnloadSceneAsync(sceneToLoad[i]);
                    break;
                }
            }

        }
    }

}
