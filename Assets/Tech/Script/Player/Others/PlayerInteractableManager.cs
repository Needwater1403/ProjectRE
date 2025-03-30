using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    public List<InteractableObject> currentInteractableObjects;

    private void Start()
    {
        currentInteractableObjects = new List<InteractableObject>();
    }

    private void FixedUpdate()
    {
        if (!PlayerUIManager.Instance.popUpWindowIsOpen && !PlayerUIManager.Instance.menuWindowIsOpen)
        {
            CheckForInteractive();
        }

        if (PlayerUIManager.Instance.popUpWindowIsOpen && !PlayerUIManager.Instance.menuWindowIsOpen)
        {
            HandleInteract();
        }
    }

    private void HandleInteract()
    {
        if(!InputManager.Instance.interactInputValue) return;
        InputManager.Instance.interactInputValue = false;
        PlayerUIManager.Instance.popUpsManager.HideAllPopUps();
        if (currentInteractableObjects.Count == 0) return;
        if (currentInteractableObjects[0] != null)
        {
            currentInteractableObjects[0].Interact(playerManager);
            Refresh();
        }
    }
    private void CheckForInteractive()
    {
        if (currentInteractableObjects.Count == 0) return;
        if (currentInteractableObjects[0] == null)
        {
            currentInteractableObjects.RemoveAt(0);
            return;
        }

        if (currentInteractableObjects[0] != null)
        {
            PlayerUIManager.Instance.popUpsManager.ShowInteractivePopUp(currentInteractableObjects[0].popUpText);
        }
    }
    
    private void Refresh()
    {
        for (var i = currentInteractableObjects.Count - 1; i > -1; i--)
        {
            if(currentInteractableObjects[i] == null) currentInteractableObjects.RemoveAt(0);
        }
    }

    public void AddInteractiveObject(InteractableObject _interactable)
    {
        Refresh();
        if(!currentInteractableObjects.Contains(_interactable)) currentInteractableObjects.Add(_interactable);
    }

    public void RemoveInteractiveObject(InteractableObject _interactable)
    {
        Refresh();
        if(currentInteractableObjects.Contains(_interactable)) currentInteractableObjects.Remove(_interactable);
        PlayerUIManager.Instance.popUpsManager.HideInteractivePopUp();
    }
}
