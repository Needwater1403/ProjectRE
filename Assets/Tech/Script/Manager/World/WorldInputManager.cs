using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class WorldInputManager : MonoBehaviour
{
    public static WorldInputManager Instance;
    [Title("InputHolder")]
    public GameObject playerInputHolder;
    public GameObject pauseMenuInputHolder;
    
    [Title("Player Input")]
    public PlayerInput playerControlInput;
    public PlayerInput pauseMenuControlInput;
    
    public InputDevice currentInputDevice;
    private string currentInputDeviceName;
    
    public GameObject lastSelectedBtn;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if(playerInputHolder == null) playerInputHolder = PlayerManager.Instance._playerInput.gameObject;
        if(playerControlInput == null) playerControlInput = PlayerManager.Instance._playerInput;
        EnablePlayerInput();
        SetCurrentControlDevice(playerControlInput);
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject != null) 
            lastSelectedBtn = EventSystem.current.currentSelectedGameObject;
        HandleInputDeviceChange();
    }

    public void DisableAll()
    {
        playerInputHolder.SetActive(false);
        pauseMenuInputHolder.SetActive(false);
    }

    #region ENABLE DEVICE
    
    public void EnablePlayerInput()
    {
        DisableAll();
        playerInputHolder.SetActive(true);
    }
    
    public void EnablePauseMenuInput()
    {
        DisableAll();
        pauseMenuInputHolder.SetActive(true);
    }
    #endregion

    #region DEVICE CHANGE

    private void HandleInputDeviceChange()
    {
        HandlePlayerInputDeviceChange();
        HandlePauseMenuInputDeviceChange();
    }
    
    private void HandlePlayerInputDeviceChange()
    {
        if(!playerInputHolder.activeInHierarchy) return;
        if(currentInputDeviceName == playerControlInput.currentControlScheme) return;
        SetCurrentControlDevice(playerControlInput);
    }
    
    private void HandlePauseMenuInputDeviceChange()
    {
        if(!pauseMenuInputHolder.activeInHierarchy) return;
        if(currentInputDeviceName == pauseMenuControlInput.currentControlScheme) return;
        SetCurrentControlDevice(pauseMenuControlInput);
    }
    
    private void SetCurrentControlDevice(PlayerInput input)
    {
        switch (input.currentControlScheme)
        {
            case "Gamepad":
                currentInputDeviceName = playerControlInput.currentControlScheme;
                currentInputDevice = InputDevice.Gamepad;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if(EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(lastSelectedBtn);
                break;
            case "Keyboard&Mouse":
                currentInputDeviceName = playerControlInput.currentControlScheme;
                currentInputDevice = InputDevice.KeyBoard_Mouse;
                if(PlayerUIManager.Instance.menuWindowIsOpen)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    return;
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
        }
    }
    
    #endregion
}
