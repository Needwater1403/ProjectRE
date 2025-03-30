using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class OneSideDoor_InteractableObject : InteractableObject
{
    [TabGroup("Interactive","One Side Door")]
    [Title("Basic Info")] 
    public string animationStateName;
    
    [TabGroup("Interactive","One Side Door")]
    [Title("Preferences")] 
    public Animator animator;
    
    [TabGroup("Interactive","One Side Door")]
    [Title("Flags")]
    public bool canBeOpen;
    [TabGroup("Interactive","One Side Door")]
    public bool isOpened;
    protected override void Start()
    {
        base.Start();
        if(CheckIsOpenedStatus())
        {
            SetInteractive(false);
            animator.Play(animationStateName);
        }
    }

    private bool CheckIsOpenedStatus()
    {
        return isOpened;
    }
    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        AttempToOpenDoor();
    }

    private void AttempToOpenDoor()
    {
        if(!canBeOpen)
        {
            WorldInputManager.Instance.EnablePauseMenuInput();
            PlayerUIManager.Instance.popUpsManager.ShowConfirmPopUp("Can Not Open From This Side");
            PlayerUIManager.Instance.popUpsManager.SetConfirmPopUpButtonFunc(ConfirmCanNotOpenMessage, null, "CONFIRM", "null", true);
        }
        else
        {
            animator.Play(animationStateName);
        }
    }

    private void ConfirmCanNotOpenMessage()
    {
        WorldInputManager.Instance.EnablePlayerInput();
        PlayerUIManager.Instance.popUpsManager.HideConfirmPopUp();
        SetInteractive(true);
    }
    public void SetCanBeOpenStatus(bool isEnable)
    {
        canBeOpen = isEnable;
    }
    
    public void SetInteractive(bool isEnable)
    {
        interactRangeCollider.enabled = isEnable;
    }
    
    public bool GetInteractive()
    {
        return interactRangeCollider.enabled;
    }
}
