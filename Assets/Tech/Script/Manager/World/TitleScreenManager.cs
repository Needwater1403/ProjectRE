using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;   
using WaitForSeconds = UnityEngine.WaitForSeconds;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager Instance;

    [Title("Version")] 
    [SerializeField] private TextMeshProUGUI versionText;
    
    [Title("Panel")] 
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Image studioLogo;
    [SerializeField] private Image blackScreen;
    
    [Title("Button")] 
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitButton;
    
    [Title("Audio")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip buttonSelectSFX;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip startGameSFX;
    [SerializeField] private AudioClip backGroundMusic;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        versionText.SetText($"App Ver. {Application.version}");
        StartCoroutine(FadeInLogoWhenStart());
    }

    public void OnClickNewGame()
    {
        SceneLoaderManager.Instance.AttemptLoadWorldScene();
    }
    
    public void OnClickQuitGame()
    {
        Application.Quit();
    }
    
    public void StartBGM()
    {
        bgmAudioSource.loop = true;
        bgmAudioSource.clip = backGroundMusic;
        bgmAudioSource.volume = 0;
        bgmAudioSource.DOFade(.5f, 10f);
        bgmAudioSource.Play();
    }
    
    public void StopBGM()
    {
        StartCoroutine(FadeOutBGM());
    }

    private IEnumerator FadeOutBGM()
    {
        bgmAudioSource.DOFade(0f, 3f);
        yield return new WaitForSeconds(3);
        bgmAudioSource.Stop();
    }
    
    private IEnumerator FadeInLogoWhenStart()
    {
        yield return new WaitForSeconds(1);
        blackScreen.DOFade(0, 2);
        yield return new WaitForSeconds(3);
        blackScreen.DOFade(1, 2);
        yield return new WaitForSeconds(3.5f);
        studioLogo.gameObject.SetActive(false);
        blackScreen.DOFade(0, 2f);
        OnStartSFX();
        yield return new WaitForSeconds(1.5f);
        StartBGM();
        yield return new WaitForSeconds(.5f);
        newGameButton.interactable = true;
        quitButton.interactable = true;
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
    }

    private SaveWriteData saveWriteData;
    
    #region BUTTONS EVENT TRIGGER

    public void OnSelectSFX()
    {
        audioSource.PlayOneShot(buttonSelectSFX);
    }
    
    public void OnClickSFX()
    {
        audioSource.PlayOneShot(buttonClickSFX);
    }
    
    public void OnStartSFX()
    {
        audioSource.PlayOneShot(startGameSFX);
    }
    #endregion
}
