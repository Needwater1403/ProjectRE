using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Message_InteractableObject : InteractableObject
{
    [TabGroup("Interactive","Message")]
    [SerializeField] private MessageContent[] messageContent;
    [Space]
    [TabGroup("Interactive","Message")]
    public string iconNameInTMP;
    [TabGroup("Interactive","Message")]
    public TMP_SpriteAsset spriteAsset;
    [Title("Options")]
    [TabGroup("Interactive", "Message")] 
    public bool isConsoleInputHas2IconCombine;
    [TabGroup("Interactive", "Message")] 
    public bool isPCInputHas2IconCombine;
    [Title("Flags")]
    [TabGroup("Interactive", "Message")] 
    public bool has2IconCombine;
    [TabGroup("Interactive", "Message")] 
    public bool isShowingThisMessageContent;
    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        isShowingThisMessageContent = true;
        var temp = "";
        // SHOW MESSAGE UI
        for (var i = 0; i < messageContent.Length; i++)
        {
            if(!messageContent[i].has2IconCombine) temp += $"{messageContent[i].prefix} <sprite name=\"{iconNameInTMP}_{i}\"> {messageContent[i].suffix}\n";
            else temp += $"{messageContent[i].prefix} <sprite name=\"{iconNameInTMP}_{i}\"> {messageContent[i].middle} " +
                         $"<sprite name=\"{iconNameInTMP}_{i}_1\"> {messageContent[i].suffix}\n";
        }
        PlayerUIManager.Instance.popUpsManager.ShowTutorialMessagePopUp(temp, spriteAsset);
    }
    
    public void SetInteractive(bool isEnable)
    {
        isShowingThisMessageContent = false;
        interactRangeCollider.enabled = isEnable;
    }
    
    public bool GetInteractive()
    {
        return interactRangeCollider.enabled;
    }

    public void SetMultipleIcons(bool isConsole)
    {
        foreach (var message in messageContent)
        {
            message.has2IconCombine = isConsole ? message.isConsoleInputHas2IconCombine : message.isPCInputHas2IconCombine;
        }
    }
}
