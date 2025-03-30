using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerUIPauseMenuManager : MonoBehaviour
{
    public CanvasGroup _canvasGroup;
    public GameObject pauseMenu;
    [Title("Text")] 
    public TextMeshProUGUI _winLabel;
    [Title("Buttons")] 
    [SerializeField] private Button returnToTitleScreenBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button exitBtn;
    [Title("Audio")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSelectSFX;
    [SerializeField] private AudioClip buttonClickSFX;
    private void Start()
    {
        returnToTitleScreenBtn.onClick.AddListener(OnClickReturnToTitleScreen);
        restartBtn.onClick.AddListener(OnClickRestart);
        exitBtn.onClick.AddListener(OnClickExitPauseMenu);
    }
    
    public void SetFirstButton()
    {
        EnableAllButton();
        EventSystem.current.SetSelectedGameObject(returnToTitleScreenBtn.gameObject);
        returnToTitleScreenBtn.Select();
    }

    private void EnableAllButton()
    {
        returnToTitleScreenBtn.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(true);
        exitBtn.gameObject.SetActive(true);
    }

    public void DisableExitButton()
    {
        exitBtn.gameObject.SetActive(false);
    }
    private void OnClickReturnToTitleScreen()
    {
        SceneLoaderManager.Instance.AttemptToReturnToTitleScreen();
    }

    private void OnClickRestart()
    {
        SceneLoaderManager.Instance.AttemptToReloadScene();
    }
    
    private void OnClickExitPauseMenu()
    {
        PlayerUIManager.Instance.SetPauseMenu(false);
    }
    
    #region BUTTONS EVENT TRIGGER

    public void OnSelectSFX()
    {
        audioSource.PlayOneShot(buttonSelectSFX);
    }
    
    public void OnClickSFX()
    {
        audioSource.PlayOneShot(buttonClickSFX);
    }
    
    #endregion
}
