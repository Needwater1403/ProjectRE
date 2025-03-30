using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TitleScreenInputManager : MonoBehaviour
{
    public static TitleScreenInputManager Instance;
    [Title("InputHolder")]
    public GameObject mainMenuInputHolder;
    
    [Title("Player Input")]
    public PlayerInput mainMenuControlInput;
    
    public InputDevice currentInputDevice;
    private string currentInputDeviceName;

    [Title("Last Selected Button")]
    [HideLabel]
    public GameObject lastSelectedBtn;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetCurrentControlDevice(mainMenuControlInput);
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject != null) 
            lastSelectedBtn = EventSystem.current.currentSelectedGameObject;
        //HandleInputDeviceChange();
    }

    public void DisableAll()
    {
        mainMenuInputHolder.SetActive(false);
    }

    #region ENABLE DEVICE
    
    public void EnableMainMenuInput()
    {
        DisableAll();
        mainMenuInputHolder.SetActive(true);
    }
    
    #endregion
    
    #region DEVICE CHANGE

    private void HandleInputDeviceChange()
    {
        HandlePlayerInputDeviceChange();
    }
    
    private void HandlePlayerInputDeviceChange()
    {
        if(!mainMenuInputHolder.activeInHierarchy) return;
        if(currentInputDeviceName == mainMenuControlInput.currentControlScheme) return;
        SetCurrentControlDevice(mainMenuControlInput);
    }
    
    private void SetCurrentControlDevice(PlayerInput input)
    {
        switch (input.currentControlScheme)
        {
            case "Gamepad":
                currentInputDeviceName = input.currentControlScheme;
                currentInputDevice = InputDevice.Gamepad;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if(EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(lastSelectedBtn);
                break;
            case "Keyboard&Mouse":
                currentInputDeviceName = input.currentControlScheme;
                currentInputDevice = InputDevice.KeyBoard_Mouse;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
    #endregion
}
