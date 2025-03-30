using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerUIPopUpsManager : MonoBehaviour
{
    [Title("YOU DIED PopUp")]
    [SerializeField] private GameObject youDiedPopUp;
    [SerializeField] private TextMeshProUGUI youDiedPopUp_text;
    [SerializeField] private TextMeshProUGUI youDiedPopUp_backgroundText;
    [SerializeField] private CanvasGroup youDiedPopUp_canvasGroup;
    
    [Title("BOSS SLAIN PopUp")]
    [SerializeField] private GameObject bossSlainPopUp;
    [SerializeField] private TextMeshProUGUI bossSlainPopUp_text;
    [SerializeField] private TextMeshProUGUI bossSlainPopUp_backgroundText;
    [SerializeField] private CanvasGroup bossSlainPopUp_canvasGroup;
    
    [Title("Message PopUp")]
    [SerializeField] private GameObject messagePopUp;
    [SerializeField] private TextMeshProUGUI messagePopUp_text;
    [SerializeField] private CanvasGroup messagePopUp_canvasGroup;
    
    [Title("Tutorial Message PopUp")]
    [SerializeField] private GameObject tutorialMessagePopUp;
    [SerializeField] private TextMeshProUGUI tutorialMessagePopUp_text;
    [SerializeField] private CanvasGroup tutorialMessagePopUp_canvasGroup;
    
    [Title("Confirm PopUp")]
    [SerializeField] private GameObject confirmPopUp;
    [SerializeField] private TextMeshProUGUI confirmPopUp_text;
    [SerializeField] private CanvasGroup confirmPopUp_canvasGroup;
    public Button confirmYes_btn;
    public Button confirmNo_btn;
    [SerializeField] private TextMeshProUGUI confirmYes_text;
    [SerializeField] private TextMeshProUGUI confirmNo_text;
    
    [Title("Loot Item PopUp")]
    [SerializeField] private GameObject lootItemPopUp;
    [SerializeField] private TextMeshProUGUI lootItemName_text;
    [SerializeField] private Image lootItemName_image;
    [SerializeField] private CanvasGroup lootItemPopUp_canvasGroup;

    #region Show Pop Up
    
    public void ShowYouDiedPopUp()
    {
        youDiedPopUp.SetActive(true);
        youDiedPopUp_backgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUp_backgroundText, 4.7f, 19f));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUp_canvasGroup, 3,2.2f,4.4f));
    }
    
    public void ShowBossSlainPopUp(string _popUpText)
    {
        bossSlainPopUp_text.SetText(_popUpText);
        bossSlainPopUp_backgroundText.SetText(_popUpText);
        bossSlainPopUp.SetActive(true);
        bossSlainPopUp_backgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(bossSlainPopUp_backgroundText, 3.7f, 19f));
        StartCoroutine(FadeInPopUpOverTime(bossSlainPopUp_canvasGroup, 2,1.2f,3.4f));
    }
    
    public void ShowInteractivePopUp(string _popUpText)
    {
        PlayerUIManager.Instance.popUpWindowIsOpen = true;
        messagePopUp.SetActive(true);
        messagePopUp_text.SetText(_popUpText);
        FadeInMessagePopUp(messagePopUp_canvasGroup, .25f);
    }
    
    public void ShowTutorialMessagePopUp(string _popUpText, TMP_SpriteAsset _spriteAsset = null)
    {
        if(_spriteAsset != null)  tutorialMessagePopUp_text.spriteAsset = _spriteAsset;
        PlayerUIManager.Instance.popUpWindowIsOpen = true;
        tutorialMessagePopUp.SetActive(true);
        tutorialMessagePopUp_text.SetText(_popUpText);
        if(tutorialMessagePopUp_canvasGroup.alpha == 0) FadeInMessagePopUp(tutorialMessagePopUp_canvasGroup, .25f);
    }
    
    public void ShowConfirmPopUp(string _popUpText)
    {
        PlayerUIManager.Instance.popUpWindowIsOpen = true;
        PlayerUIManager.Instance.menuWindowIsOpen = true;
        confirmPopUp.SetActive(true);
        confirmPopUp_text.SetText(_popUpText);
        FadeInMessagePopUp(confirmPopUp_canvasGroup, .25f);
        confirmYes_btn.Select();
        if (WorldInputManager.Instance.currentInputDevice != InputDevice.KeyBoard_Mouse) return;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void ShowItemPopUp(string _popUpText, Sprite itemSprite)
    {
        PlayerUIManager.Instance.popUpWindowIsOpen = true;
        lootItemPopUp.SetActive(true);
        lootItemName_text.SetText(_popUpText);
        lootItemName_image.sprite = itemSprite;
        FadeInMessagePopUp(lootItemPopUp_canvasGroup, .25f);
    }
    

    #endregion

    #region Hide Pop Up

    public void HideAllPopUps()
    {
        HideInteractivePopUp();
        HideItemPopUp();
        HideConfirmPopUp();
        HideTutorialMessagePopUp();
    }
    
    public void HideInteractivePopUp()
    {
        PlayerUIManager.Instance.popUpWindowIsOpen = false;
        if(messagePopUp_canvasGroup.alpha != 0) FadeOutMessagePopUp(messagePopUp_canvasGroup, .25f);
    }
    
    public void HideConfirmPopUp()
    {
        PlayerUIManager.Instance.popUpWindowIsOpen = false;
        PlayerUIManager.Instance.menuWindowIsOpen = false;
        if(confirmPopUp_canvasGroup.alpha != 0) FadeOutMessagePopUp(confirmPopUp_canvasGroup, .25f);
        confirmYes_btn.onClick.RemoveAllListeners();
        confirmNo_btn.onClick.RemoveAllListeners();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void HideTutorialMessagePopUp()
    {
        PlayerUIManager.Instance.popUpWindowIsOpen = false;
        if(tutorialMessagePopUp_canvasGroup.alpha != 0) FadeOutMessagePopUp(tutorialMessagePopUp_canvasGroup, .25f);
    }
    
    public void HideItemPopUp()
    {
        PlayerUIManager.Instance.popUpWindowIsOpen = false;
        if(lootItemPopUp_canvasGroup.alpha != 0) FadeOutMessagePopUp(lootItemPopUp_canvasGroup, .25f);
        
    }
    

    #endregion
    
    
    public void SetConfirmPopUpButtonFunc(UnityAction yesAction = null, UnityAction noAction = null, string yesText = null, string noText = null, bool showOnlyOneBtn = false)
    {
        confirmYes_btn.onClick.RemoveAllListeners();
        confirmNo_btn.onClick.RemoveAllListeners();
        
        if (yesAction == null && noAction == null)
        {
            confirmYes_text.SetText("CONFIRM");
            confirmNo_btn.gameObject.SetActive(false);
            confirmYes_btn.onClick.AddListener(HideConfirmPopUp);
            return;
        }

        confirmYes_btn.gameObject.SetActive((showOnlyOneBtn && yesAction != null)||!showOnlyOneBtn);
        confirmNo_btn.gameObject.SetActive((!showOnlyOneBtn && noAction != null)||!showOnlyOneBtn);
        confirmYes_text.SetText(yesText.IsNullOrWhitespace()? "YES" : yesText);
        confirmNo_text.SetText(noText.IsNullOrWhitespace()? "NO" : noText);
        confirmYes_btn.onClick.AddListener(yesAction ?? HideConfirmPopUp);
        confirmNo_btn.onClick.AddListener(noAction ?? HideConfirmPopUp);
    }
    
    public void SelectConfirmPopUpButton_Confirm()
    {
        confirmYes_btn.Select();
    }
    public void SelectConfirmPopUpButton_Cancel()
    {
        confirmNo_btn.Select();
    }
    
    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration > 0f)
        {
            text.characterSpacing = 0;
            float timer = 0;
            yield return null;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                text.characterSpacing =
                    Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }
    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvasGroup, float fadeInDuration, float fadeOutDuration, float waitTime)
    {
        canvasGroup.DOFade(1, fadeInDuration);
        yield return new WaitForSeconds(waitTime);
        canvasGroup.DOFade(0, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);
        canvasGroup.gameObject.SetActive(false);
    }
    
    private void FadeInMessagePopUp(CanvasGroup canvasGroup, float fadeInDuration)
    {
        canvasGroup.DOFade(1, fadeInDuration);
    }
    
    private void FadeOutMessagePopUp(CanvasGroup canvasGroup, float fadeOutDuration)
    {
        StartCoroutine(IFadeOutMessagePopUp(canvasGroup, fadeOutDuration));
    }
    
    private IEnumerator IFadeOutMessagePopUp(CanvasGroup canvasGroup, float fadeOutDuration)
    {
        canvasGroup.DOFade(0, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);
        canvasGroup.gameObject.SetActive(false);
    }
}
