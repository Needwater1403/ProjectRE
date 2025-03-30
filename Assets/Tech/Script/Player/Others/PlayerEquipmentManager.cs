using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using VFolders.Libs;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    private PlayerManager _playerManager;
    
    [TabGroup("Equipment","Weapon")]
    [Title("Weapon Model Instantiation Slot")]
    public WeaponModelInstantiationSlot rightHandWeaponSlot;
    [TabGroup("Equipment","Weapon")]
    public WeaponModelInstantiationSlot rightHipsWeaponSlot;
    [TabGroup("Equipment","Weapon")]
    public WeaponModelInstantiationSlot backWeaponSlot;
    
    [Title("Weapon Manager")] 
    [TabGroup("Equipment","Weapon")]
    public WeaponManager rightHandWeaponManager;
    
    private GameObject rightHandWeaponModel;
    
    protected override void Awake()
    {
        base.Awake();
        _playerManager = GetComponent<PlayerManager>();
        InitWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();
        HandleLoadWeapon();
        //LoadConsumable();
    }

    #region WEAPONS

    private void InitWeaponSlots()
    {
        var weaponSlotList = GetComponentsInChildren<WeaponModelInstantiationSlot>();
        foreach (var w in weaponSlotList)
        {
            switch (w.modelSlotType)
            {
                case WeaponModelSlot.RightHandWeapon:
                    rightHandWeaponSlot = w;
                    break;
                case WeaponModelSlot.Back:
                    backWeaponSlot = w;
                    break;
                case WeaponModelSlot.RightHips:
                    rightHipsWeaponSlot = w;
                    break;
                case WeaponModelSlot.Empty:
                    break;
            }
        }
    }

    public void HandleLoadWeapon()
    {
        _playerManager._controlInventory.SetCurrentWeaponOnStart();
        LoadRightHandWeapon();
    }

    public void HandleAllSwitch()
    {
        // WEAPONS
        SwitchWeapon();
        
        // CONSUMABLES
        //HandleSwitchConsumable();
    }

    #region Load Weapon

    private bool isBowLeftHand;
    private bool isBowRightHand;
    private bool isShowProjectileUI;
    public void LoadRightHandWeapon()
    {
        // LOAD WEAPON
        if (_playerManager._controlInventory.currentWeaponItem != null)
        {
            rightHandWeaponSlot.UnloadWeapon();
            
            if(_playerManager._controlInventory.currentWeaponItem.weaponModel!=null)
            {
                rightHandWeaponModel =
                    Instantiate(_playerManager._controlInventory.currentWeaponItem.weaponModel);
            }
            switch (_playerManager._controlInventory.currentWeaponItem.weaponType)
            {
                case WeaponType.Mace:
                    _playerManager._controlCombat.isUsingMeleeWeapon = true;
                    rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel, _playerManager._controlInventory.currentWeaponItem.weaponType);
                    break;
                case WeaponType.Bow:
                    _playerManager._controlCombat.isUsingMeleeWeapon = false;
                    rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel, _playerManager._controlInventory.currentWeaponItem.weaponType);
                    break;
                case WeaponType.Pistol:
                    _playerManager._controlCombat.isUsingMeleeWeapon = false;
                    rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel, _playerManager._controlInventory.currentWeaponItem.weaponType);
                    break;
                case WeaponType.Rifle:
                    _playerManager._controlCombat.isUsingMeleeWeapon = false;
                    rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel, _playerManager._controlInventory.currentWeaponItem.weaponType);
                    break;
                case WeaponType.Launcher:
                    _playerManager._controlCombat.isUsingMeleeWeapon = false;
                    rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel, _playerManager._controlInventory.currentWeaponItem.weaponType);
                    break;
                case WeaponType.Shotgun:
                    _playerManager._controlCombat.isUsingMeleeWeapon = false;
                    rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel, _playerManager._controlInventory.currentWeaponItem.weaponType);
                    break;
                case WeaponType.Knuckle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            rightHandWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            if(rightHandWeaponManager!=null) rightHandWeaponManager.SetWeaponDamage(_playerManager._controlInventory.currentWeaponItem, _playerManager);
            _playerManager._controlAnimator.UpdateAnimatorController(_playerManager._controlInventory.currentWeaponItem);
            
            
            if(PlayerUIManager.Instance != null)
            {
                // HANDLE UI
                PlayerUIManager.Instance.hubManager.SetRightWeaponSlot(_playerManager._controlInventory.currentWeaponItem.ID);
                if (!_playerManager._controlCombat.isUsingMeleeWeapon)
                {
                    var rangedWeapon = _playerManager._controlInventory.currentWeaponItem as RangedWeaponItem;
                    _playerManager._controlInventory.currentAmmoCapacity = rangedWeapon.ammoCapacity;
                    _playerManager._controlInventory.currentAmmoAmountInMag =
                        _playerManager._controlInventory.ammoAmountInMagList[_playerManager._controlInventory.rightHandWeaponIndex];
                    PlayerUIManager.Instance.hubManager.SetAmmoText(_playerManager._controlInventory.currentAmmoAmountInMag, _playerManager._controlInventory.currentAmmoItem.amount);
                }
                else PlayerUIManager.Instance.hubManager.DisableAmmoText();
                
            }
        }
    }
    
    #endregion
    
    #region Switch Weapon
    
    public void SwitchWeapon()
    {
        // CAN ONLY SWITCH ON THESE CONDITION
        if (!InputManager.Instance.switchWeaponInputValue) return;
        
        // PLAY ANIMATION
        InputManager.Instance.switchWeaponInputValue = false;
        if (_playerManager.isDoingAction || _playerManager.isJumping)return;
        
        _playerManager._controlAnimator.PlayActionAnimation(Constants.PlayerAnimation_Switch_Right_Hip_Weapon_01, 
            false, false, true, true);
        WeaponItem selectWeaponItem = null;

        // CHECK FOR UNARMED WEAPON ITEM COUNT ON LEFT HAND
        _playerManager._controlInventory.rightHandWeaponUnarmedCount = 0;
        foreach (var w in _playerManager._controlInventory.rightHandWeaponItemQuickSlots)
        {
            if (w.ID == ItemDatabase.Instance.UnarmedWeaponItem.ID)
                _playerManager._controlInventory.rightHandWeaponUnarmedCount++;
        }
        
        // INCREASE INDEX
        _playerManager._controlInventory.rightHandWeaponIndex += 1;
        Debug.Log(_playerManager._controlInventory.rightHandWeaponIndex);
        if (_playerManager._controlInventory.rightHandWeaponIndex is < 0 or > 5)
        {
            _playerManager._controlInventory.rightHandWeaponIndex = 0;
        }
        selectWeaponItem = _playerManager._controlInventory.rightHandWeaponItemQuickSlots[_playerManager._controlInventory.rightHandWeaponIndex];
        
        // SWITCH WEAPON RULES (OLD ONE USES FOR SOULS LIKE GAME WITH UNARMED WEAPON)
        #region Old Switch Weapon Rules

        switch (_playerManager._controlInventory.rightHandWeaponUnarmedCount)
        {
            case 0: // IF NO UNARMED -> SWITCH LIKE NORMAL
                selectWeaponItem = _playerManager._controlInventory.rightHandWeaponItemQuickSlots[_playerManager._controlInventory.rightHandWeaponIndex];
                break;
            case 1: // IF 1 UNARMED -> SKIP THE UNARMED
                if (_playerManager._controlInventory.rightHandWeaponItemQuickSlots[_playerManager._controlInventory.rightHandWeaponIndex].ID !=
                    ItemDatabase.Instance.UnarmedWeaponItem.ID)
                {
                    selectWeaponItem = _playerManager._controlInventory.rightHandWeaponItemQuickSlots[_playerManager._controlInventory.rightHandWeaponIndex];
            
                }
                else
                {
                    _playerManager._controlInventory.rightHandWeaponIndex += 1;
                    if (_playerManager._controlInventory.rightHandWeaponIndex is < 0 or > 2)
                    {
                        _playerManager._controlInventory.rightHandWeaponIndex = 0;
                    }
                    selectWeaponItem = _playerManager._controlInventory.rightHandWeaponItemQuickSlots[_playerManager._controlInventory.rightHandWeaponIndex];
                }
                break;
            case 2: // IF 2 UNARMED -> SWITCH BETWEEN 1 UNARMED AND THE ONLY WEAPON ITEM
                if (_playerManager._controlInventory.rightHandWeaponItemQuickSlots[_playerManager._controlInventory.rightHandWeaponIndex].ID !=
                    ItemDatabase.Instance.UnarmedWeaponItem.ID)
                {
                    selectWeaponItem = _playerManager._controlInventory.rightHandWeaponItemQuickSlots[_playerManager._controlInventory.rightHandWeaponIndex];
                }
                else
                {
                    if(_playerManager._controlInventory.currentWeaponItem.ID !=
                       ItemDatabase.Instance.UnarmedWeaponItem.ID)
                    {
                        selectWeaponItem = _playerManager._controlInventory.rightHandWeaponItemQuickSlots[_playerManager._controlInventory.rightHandWeaponIndex];
                    }
                    else
                    {
                        _playerManager._controlInventory.rightHandWeaponIndex += 1;
                        if (_playerManager._controlInventory.rightHandWeaponIndex is < 0 or > 2)
                        {
                            _playerManager._controlInventory.rightHandWeaponIndex = 0;
                        }
                        selectWeaponItem = _playerManager._controlInventory.rightHandWeaponItemQuickSlots[_playerManager._controlInventory.rightHandWeaponIndex];
                    }
                }
                break;
        }

        #endregion

        if (selectWeaponItem != null)
        {
            _playerManager._controlInventory.currentWeaponItem = selectWeaponItem;
            _playerManager._controlInventory.currentAmmoItem = _playerManager._controlInventory.GetAmmoBasedOnWeapon(selectWeaponItem as RangedWeaponItem);
        }
        
        // LOAD WEAPON
        LoadRightHandWeapon();  
    }
    
    #endregion

    #region Damage Colliders

    public void EnableCollider()
    {
        // ENABLE RIGHT HAND COLLIDER
        rightHandWeaponManager._damageCollider.SetCollider(true);
        _playerManager._controlSoundFXBase.PlaySFX(_playerManager._controlInventory.currentWeaponItem.AttackSFX.GetRandom(),.45f);
    }
    
    public void DisableCollider()
    {
        // DISABLE RIGHT HAND COLLIDER
        rightHandWeaponManager._damageCollider.SetCollider(false);
        rightHandWeaponManager._damageCollider.ClearTargetList();
    }

    #endregion

    #region TWO HAND

    // public void MoveLeftHandWeaponModelWhenTwoHand(WeaponModelInstantiationSlot slot)
    // {
    //     slot.MoveWeaponModelToSpecificSlot(leftHandWeaponModel, _playerManager._controlInventory.currentLeftHandWeaponItem.weaponType, _playerManager);
    //     rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel, _playerManager._controlInventory.currentTwoHandWeaponItem.weaponType, true);
    // }
    //
    // public void MoveRightHandWeaponModelWhenTwoHand(WeaponModelInstantiationSlot slot)
    // {
    //     slot.MoveWeaponModelToSpecificSlot(rightHandWeaponModel, _playerManager._controlInventory.currentRightHandWeaponItem.weaponType, _playerManager);
    //     rightHandWeaponSlot.LoadWeapon(leftHandWeaponModel, _playerManager._controlInventory.currentTwoHandWeaponItem.weaponType, true);
    // }
    //
    // public void MoveWeaponModelWhenUnTwoHand()
    // { 
    //     if(_playerManager._controlInventory.currentRightHandWeaponItem.weaponType is WeaponType.MediumShield or WeaponType.LightShield)
    //     {
    //         rightHandShieldSlot.LoadWeapon(rightHandWeaponModel, 
    //             _playerManager._controlInventory.currentRightHandWeaponItem.weaponType);
    //     }
    //     else
    //     {
    //         rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel, 
    //             _playerManager._controlInventory.currentRightHandWeaponItem.weaponType);
    //     }
    //
    //     if(_playerManager._controlInventory.currentLeftHandWeaponItem.weaponType is WeaponType.MediumShield or WeaponType.LightShield)
    //     {
    //         leftHandShieldSlot.LoadWeapon(leftHandWeaponModel,
    //             _playerManager._controlInventory.currentLeftHandWeaponItem.weaponType);
    //     }
    //     else
    //     {
    //         Debug.Log(leftHandWeaponModel.name);
    //         leftHandWeaponSlot.LoadWeapon(leftHandWeaponModel,
    //             _playerManager._controlInventory.currentLeftHandWeaponItem.weaponType);
    //     }
    // }

    #endregion

    public void HideRightWeaponModel()
    {
        rightHandWeaponModel.SetActive(false);
    }
    
    public void ShowRightWeaponModel()
    {
        rightHandWeaponModel.SetActive(true);
    }
    
    #endregion
    
    
    #region CONSUMABLE

    // public void LoadConsumable()
    // {
    //     if (_playerManager._controlInventory.consumableItemQuickSlots[_playerManager._controlInventory.consumableIndex] == null)
    //     {
    //         _playerManager._controlInventory.currentConsumable = null;
    //         if(PlayerUIManager.Instance != null) PlayerUIManager.Instance.hubManager.SetConsumableSlot(-1,0);
    //         return;
    //     }
    //     _playerManager._controlInventory.currentConsumable =
    //         _playerManager._controlInventory.consumableItemQuickSlots[_playerManager._controlInventory.consumableIndex];
    //     if(PlayerUIManager.Instance != null) 
    //         PlayerUIManager.Instance.hubManager.SetConsumableSlot(_playerManager._controlInventory.currentConsumable.ID, _playerManager._controlInventory.currentConsumable.amount);
    // }
    //
    // public void HandleSwitchConsumable()
    // {
    //     // CAN ONLY SWITCH ON THESE CONDITION
    //     if (!InputManager.Instance.switchConsumableInputValue) return;
    //     InputManager.Instance.switchConsumableInputValue = false;
    //     
    //     // INCREASE INDEX
    //     for (var i = _playerManager._controlInventory.consumableIndex; i < _playerManager._controlInventory.consumableItemQuickSlots.Length; i++)
    //     {
    //         _playerManager._controlInventory.consumableIndex++;
    //         if (_playerManager._controlInventory.consumableIndex >= _playerManager._controlInventory.consumableItemQuickSlots.Length)
    //         {
    //             _playerManager._controlInventory.consumableIndex = 0;
    //         }
    //
    //         if (_playerManager._controlInventory.consumableItemQuickSlots[
    //                 _playerManager._controlInventory.consumableIndex] != null) break;
    //     }
    //     
    //     LoadConsumable();
    // }

    #endregion

    #region PROJECTILE

    // public void LoadProjectile(int index)
    // {
    //     switch (index)
    //     {
    //         case 0:
    //             if(PlayerUIManager.Instance == null) return;
    //             if(_playerManager._controlInventory.mainArrowItem == null) 
    //                 PlayerUIManager.Instance.hubManager.SetProjectileSlot(-1,0);
    //             else PlayerUIManager.Instance.hubManager.SetProjectileSlot(_playerManager._controlInventory.mainArrowItem.ID, _playerManager._controlInventory.mainArrowItem.amount);
    //             break;
    //         case 1:
    //             if(PlayerUIManager.Instance == null) return;
    //             if(_playerManager._controlInventory.subArrowItem == null) 
    //                 PlayerUIManager.Instance.hubManager.SetSubProjectileSlot(-1,0);
    //             else PlayerUIManager.Instance.hubManager.SetSubProjectileSlot(_playerManager._controlInventory.subArrowItem.ID, _playerManager._controlInventory.subArrowItem.amount);
    //             break;
    //         case 2:
    //             if(PlayerUIManager.Instance == null) return;
    //             if(_playerManager._controlInventory.mainBoltItem == null) 
    //                  PlayerUIManager.Instance.hubManager.SetProjectileSlot(-1,0);
    //             else PlayerUIManager.Instance.hubManager.SetProjectileSlot(_playerManager._controlInventory.mainBoltItem.ID, _playerManager._controlInventory.mainBoltItem.amount);
    //             break;
    //         case 3:
    //             if(PlayerUIManager.Instance == null) return;
    //             if(_playerManager._controlInventory.subBoltItem == null) 
    //                 PlayerUIManager.Instance.hubManager.SetSubProjectileSlot(-1,0);
    //             else PlayerUIManager.Instance.hubManager.SetSubProjectileSlot(_playerManager._controlInventory.subBoltItem.ID, _playerManager._controlInventory.subBoltItem.amount);
    //             break;
    //     }
    // }

    #endregion

}