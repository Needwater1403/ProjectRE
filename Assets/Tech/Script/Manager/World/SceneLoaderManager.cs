using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    
    [Button("Return to Title Screen")]
    public void AttemptToReturnToTitleScreen()
    {
        StartCoroutine(ReturnToTitleScreen());
    }

    [Button("Reload Screen")]
    public void AttemptToReloadScene()
    {
        StartCoroutine(ReloadScene());
    }

    public void AttemptLoadWorldScene()
    {
        StartCoroutine(LoadWorldScene());
    }
    
    private IEnumerator LoadWorldScene()
    {
        TransitionManager.Instance.SetLoadingScreen(true);
        TitleScreenManager.Instance.StopBGM();
        yield return new WaitForSeconds(3f);
        var persistentDataAsync = SceneManager.LoadSceneAsync(sceneBuildIndex: 1);
        while (!persistentDataAsync.isDone)
        {
            TransitionManager.Instance.SetProgressBar(persistentDataAsync.progress);
            yield return null;
        }
        PlayerManager.Instance.SetFreezeStatus(true);
        TransitionManager.Instance.SetLoadingScreen(false);
    }
    private IEnumerator ReturnToTitleScreen()
    {
        TransitionManager.Instance.SetLoadingScreen(true);
        yield return new WaitForSeconds(1);
        SceneManager.UnloadSceneAsync(1);
        var asyncOperations = SceneManager.LoadSceneAsync(0,LoadSceneMode.Single);
        while (!asyncOperations.isDone)
        {
            TransitionManager.Instance.SetProgressBar(asyncOperations.progress);
            yield return null;
        }
        TransitionManager.Instance.SetLoadingScreen(false);
    }
    
    private IEnumerator ReloadScene()
    {
        PlayerManager.Instance.SetFreezeStatus(true);
        TransitionManager.Instance.SetLoadingScreen(true);
        yield return new WaitForSeconds(1);
        
        SceneManager.UnloadSceneAsync(1);
        var asyncOperations = SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
        while (!asyncOperations.isDone)
        {
            TransitionManager.Instance.SetProgressBar(asyncOperations.progress);
            yield return null;
        }
        TransitionManager.Instance.SetLoadingScreen(false);
    }
}
