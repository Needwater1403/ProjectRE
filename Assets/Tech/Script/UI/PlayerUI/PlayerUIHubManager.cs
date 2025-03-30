using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIHubManager : MonoBehaviour
{
    [Title("Stat Bars")]
    [SerializeField] private UI_StatBar healthBar;
    [SerializeField] private UI_StatBar staminaBar;
    
    [Title("Quick Slots Image")]
    [SerializeField] private Image rightWeapons_QuickSlot;
    
    [Title("CrossHair")]
    [SerializeField] private GameObject pistolCrossHair;
    [SerializeField] private GameObject rifleCrossHair;
    [SerializeField] private GameObject launcherCrossHair;
    [SerializeField] private GameObject shotgunCrossHair;
    
    [Title("Ammo")]
    [SerializeField] private TextMeshProUGUI ammoText;
    
    [Title("Pills")]
    [SerializeField] private TextMeshProUGUI pillsText;

    [Title("Boss UI")] 
    [SerializeField] private GameObject bossUIHolder;
    [SerializeField] private UI_BossHPBar bossHPBar;
    [SerializeField] private TextMeshProUGUI bossName_Txt;
    [SerializeField] private TextMeshProUGUI bossHP_Txt;
    #region CROSSHAIR FUNC

    public void ShowCrossHair(WeaponType weaponType)
    {
        HideAllCrossHair();
        switch (weaponType)
        {
            case WeaponType.Pistol or WeaponType.Bow:
                pistolCrossHair.SetActive(true);
                break;
            case WeaponType.Rifle:
                rifleCrossHair.SetActive(true);
                break;
            case WeaponType.Launcher:
                launcherCrossHair.SetActive(true);
                break;
            case WeaponType.Shotgun:
                shotgunCrossHair.SetActive(true);
                break;
        }
    }
    
    public void HideAllCrossHair()
    {
        pistolCrossHair.SetActive(false);
        rifleCrossHair.SetActive(false);
        launcherCrossHair.SetActive(false);
        shotgunCrossHair.SetActive(false);
    }

    #endregion

    #region AMMO FUNC

    public void SetAmmoText(int currentAmmo, int maxAmmo)
    {
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }
    
    public void DisableAmmoText()
    {
        ammoText.text = "";
    }

    #endregion
    
    public void SetPillsText(int currentValue)
    {
        pillsText.text = currentValue.ToString();
    }
    
    #region STAT BAR FUNC
    
    public void SetNewStaminaValue(float oldValue, float newValue, float maxValue)
    {
        staminaBar.SetStat(newValue/maxValue);
        if(oldValue > newValue) PlayerManager.Instance._controlStatsBase.ResetStaminaRegenTimer();
    }
    
    public void SetMaxStaminaValue(float maxValue)
    {
        staminaBar.SetMaxStat(maxValue);
    }

    public void SetNewHealthValue(float newValue)
    {
        healthBar.SetStat(newValue);
    }
    
    public void SetMaxHealthValue(float maxValue)
    {
        healthBar.SetMaxStat(maxValue);
    }
    
    #endregion

    #region ITEM QUICK SLOTS FUNC

    public void SetRightWeaponSlot(int weaponID)
    {
        var weapon = ItemDatabase.Instance.GetWeaponByID(weaponID);
        if (weapon == null)
        {
            rightWeapons_QuickSlot.gameObject.SetActive(false);
            rightWeapons_QuickSlot.enabled = false;
            Debug.Log("NO R WEAPON FOUND");
            return;
        }
        if (weapon.icon == null) 
        {
            rightWeapons_QuickSlot.gameObject.SetActive(false);
            rightWeapons_QuickSlot.enabled = false;
            Debug.Log("NO R WEAPON ICON FOUND");
            return;
        }
        
        rightWeapons_QuickSlot.gameObject.SetActive(true);
        rightWeapons_QuickSlot.enabled = true;
        rightWeapons_QuickSlot.sprite = weapon.icon;
    }
    
    #endregion

    #region BOSS UI FUNC

    public void SetBossUIStatus(AIBossCharacterManager aiBossCharacterManager, bool isEnable)
    {
        if(isEnable)
        {
            bossHPBar.EnableBossHPBar(aiBossCharacterManager);
            bossName_Txt.SetText(aiBossCharacterManager.bossName);
            bossHP_Txt.SetText($"{bossHPBar.HpOwnerManager._controlStatsBase.currentHealth} / {bossHPBar.HpOwnerManager._controlStatsBase.maxHealth}");
            bossUIHolder.SetActive(true);
        }
        else bossUIHolder.SetActive(false);
    }
    public void SetBossNewHealthValue(float oldValue, float newValue)
    {
        bossHPBar.SetStat(newValue);
        bossHP_Txt.SetText($"{bossHPBar.HpOwnerManager._controlStatsBase.currentHealth} / {bossHPBar.HpOwnerManager._controlStatsBase.maxHealth}");
    }
    
    public bool IsInBossEvent()
    {
        return bossUIHolder.activeInHierarchy;
    }
    
    public AIBossCharacterManager GetBossManager()
    {
        return bossHPBar.HpOwnerManager;
    }
    #endregion
}
