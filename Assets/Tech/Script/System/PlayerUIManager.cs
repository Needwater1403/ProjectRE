using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;
    [Title("Preferences")] 
    public PlayerUIHubManager hubManager;
    public PlayerUIPopUpsManager popUpsManager;
    public PlayerUIPauseMenuManager pauseMenuManager;
    
    [Title("Flags")] 
    public bool menuWindowIsOpen = false;
    public bool popUpWindowIsOpen = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public void SetPlayerUIOnStart()
    {
        hubManager.SetMaxStaminaValue(PlayerManager.Instance._controlStatsBase.maxStamina);
        hubManager.SetMaxHealthValue(PlayerManager.Instance._controlStatsBase.maxHealth);
    }
    
    public void SetPauseMenu(bool _isEnable)
    {
        if(!_isEnable)
        {
            StartCoroutine(FadeOutSelectMenu());
        }
        else
        {   
            WorldInputManager.Instance.EnablePauseMenuInput();
            menuWindowIsOpen = true;
            popUpWindowIsOpen = false;
            pauseMenuManager.pauseMenu.SetActive(true);
            hubManager.gameObject.SetActive(false);
            pauseMenuManager._canvasGroup.DOFade(1, .2f);
            pauseMenuManager.SetFirstButton();
            if (WorldInputManager.Instance.currentInputDevice != InputDevice.KeyBoard_Mouse) return;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    private IEnumerator FadeOutSelectMenu()
    {
        pauseMenuManager._canvasGroup.DOFade(0, .2f);
        yield return new WaitForSeconds(.2f);
        pauseMenuManager._winLabel.gameObject.SetActive(true);
        pauseMenuManager.pauseMenu.SetActive(false);
        hubManager.gameObject.SetActive(true);
        menuWindowIsOpen = false;
        WorldInputManager.Instance.EnablePlayerInput();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
