using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class KeyDoor_InteractableObject : InteractableObject
{
    [TabGroup("Interactive","Key Door")]
    [Title("Basic Info")] 
    public string animationStateName;
    
    [TabGroup("Interactive","Key Door")]
    [Title("Preferences")] 
    public Animator animator;
    [TabGroup("Interactive", "Key Door")] 
    public List<Item> keys;
    
    [TabGroup("Interactive","Key Door")]
    [Title("Flags")]
    public bool canBeOpen;
    [TabGroup("Interactive","Key Door")]
    public bool isOpened;
    protected override void Start()
    {
        base.Start();
        if(CheckIsOpenedStatus())
        {
            SetInteractive(false);
            animator.Play(animationStateName);animator.Play(animationStateName);
        }
    }

    private bool CheckIsOpenedStatus()
    {
        return isOpened;
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        AttemptToOpenDoor();
    }

    private void AttemptToOpenDoor()
    {
        canBeOpen = CheckIfWeHaveKeyForThisDoor();
        if(!canBeOpen)
        {
            WorldInputManager.Instance.EnablePauseMenuInput();
            PlayerUIManager.Instance.popUpsManager.ShowConfirmPopUp("Locked");
            PlayerUIManager.Instance.popUpsManager.SetConfirmPopUpButtonFunc(ConfirmCanNotOpenMessage, null, "CONFIRM", null, true);
        }
        else
        {
            SetInteractive(false);
            animator.Play(animationStateName);
        }
    }
    
    private void ConfirmCanNotOpenMessage()
    {
        WorldInputManager.Instance.EnablePlayerInput();
        PlayerUIManager.Instance.popUpsManager.HideConfirmPopUp();
        SetInteractive(true);
    }

    private bool CheckIfWeHaveKeyForThisDoor()
    {
        return keys.Any(key => PlayerManager.Instance._controlInventory.inventoryItems.Any(item => item.ID == key.ID));
    }
    public void SetInteractive(bool isEnable)
    {
        interactRangeCollider.enabled = isEnable;
    }
}
