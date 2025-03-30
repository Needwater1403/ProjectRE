using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AICharacterEquipmentManager : CharacterEquipmentManager
{
    private AICharacterManager aiCharacterManager;
    
    [Title("Weapon Model Instantiation Slot")]
    public WeaponModelInstantiationSlot rightHandWeaponSlot;
    public WeaponModelInstantiationSlot leftHandWeaponSlot;
    public WeaponModelInstantiationSlot leftHipsWeaponSlot;
    public WeaponModelInstantiationSlot rightHipsWeaponSlot;
    public WeaponModelInstantiationSlot backWeaponSlot;

    [Title("Weapon Manager")] 
    public WeaponManager leftHandWeaponManager;
    public WeaponManager rightHandWeaponManager;
    
    private GameObject leftHandWeaponModel;
    private GameObject rightHandWeaponModel;
    
    protected override void Awake()
    {
        base.Awake();
        aiCharacterManager = GetComponent<AICharacterManager>();
        InitWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();
        HandleLoadWeaponOnBothHands();
    }
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
                case WeaponModelSlot.LeftHips:
                    leftHipsWeaponSlot = w;
                    break;
                case WeaponModelSlot.RightHips:
                    rightHipsWeaponSlot = w;
                    break;
                case WeaponModelSlot.Empty:
                    break;
            }
        }
    }

    public void HandleLoadWeaponOnBothHands()
    {
        LoadLeftHandWeapon();
        LoadRightHandWeapon();
    }

    public void HandleAllSwitch()
    {
        // WEAPONS
        SwitchLeftHandWeapon();
        SwitchRightHandWeapon();
    }

    #region Load

    public void LoadLeftHandWeapon()
    {
        if (aiCharacterManager._controlInventory.currentLeftHandWeaponItem != null)
        {
            if(leftHandWeaponModel!=null) Destroy(leftHandWeaponModel);
            if(aiCharacterManager._controlInventory.currentLeftHandWeaponItem.weaponModel!=null)
            {
                leftHandWeaponModel =
                    Instantiate(aiCharacterManager._controlInventory.currentLeftHandWeaponItem.weaponModel);
            }
            leftHandWeaponSlot.LoadWeapon(leftHandWeaponModel, aiCharacterManager._controlInventory.currentLeftHandWeaponItem.weaponType);
            leftHandWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            if(leftHandWeaponManager!=null) leftHandWeaponManager.SetWeaponDamage(aiCharacterManager._controlInventory.currentLeftHandWeaponItem, aiCharacterManager);
            //aiCharacterManager._controlAnimatorBase.UpdateAnimatorController(aiCharacterManager._controlInventory.currentLeftHandWeaponItem);
        }
    }
    public void LoadRightHandWeapon()
    {
        if (aiCharacterManager._controlInventory.currentRightHandWeaponItem != null)
        {
            if(rightHandWeaponModel!=null) Destroy(rightHandWeaponModel);
            if(aiCharacterManager._controlInventory.currentRightHandWeaponItem.weaponModel!=null)
            {
                rightHandWeaponModel =
                    Instantiate(aiCharacterManager._controlInventory.currentRightHandWeaponItem.weaponModel);
            }
            rightHandWeaponSlot.LoadWeapon(rightHandWeaponModel, aiCharacterManager._controlInventory.currentRightHandWeaponItem.weaponType);
            rightHandWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            if(rightHandWeaponManager!=null) rightHandWeaponManager.SetWeaponDamage(aiCharacterManager._controlInventory.currentRightHandWeaponItem, aiCharacterManager);
            //aiCharacterManager._controlAnimatorBase.UpdateAnimatorController(aiCharacterManager._controlInventory.currentRightHandWeaponItem);
        }
    }
    
    #endregion
    
    #region Switch
    
    public void SwitchLeftHandWeapon()
    {
        // CAN ONLY SWITCH ON THESE CONDITION
        if (aiCharacterManager.isDoingAction || aiCharacterManager.isJumping)return;
        aiCharacterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Switch_Left_Hip_Weapon_01, 
                                                            false, true, true, true);
        WeaponItem selectWeaponItem = null;
        
        // CHECK FOR UNARMED WEAPON ITEM COUNT ON LEFT HAND
        aiCharacterManager._controlInventory.leftHandWeaponUnarmedCount = 0;
        foreach (var w in aiCharacterManager._controlInventory.leftHandWeaponItemQuickSlots)
        {
            if (w.ID == ItemDatabase.Instance.UnarmedWeaponItem.ID)
                aiCharacterManager._controlInventory.leftHandWeaponUnarmedCount++;
        }
        
        // INCREASE INDEX
        aiCharacterManager._controlInventory.leftHandWeaponIndex += 1;
        if (aiCharacterManager._controlInventory.leftHandWeaponIndex is < 0 or > 2)
        {
            aiCharacterManager._controlInventory.leftHandWeaponIndex = 0;
        }
        
        // SWITCH WEAPON RULES
        switch (aiCharacterManager._controlInventory.leftHandWeaponUnarmedCount)
        {
            case 0: // IF NO UNARMED -> SWITCH LIKE NORMAL
                selectWeaponItem = aiCharacterManager._controlInventory.leftHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.leftHandWeaponIndex];
                break;
            case 1: // IF 1 UNARMED -> SKIP THE UNARMED
                if (aiCharacterManager._controlInventory.leftHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.leftHandWeaponIndex].ID !=
                    ItemDatabase.Instance.UnarmedWeaponItem.ID)
                {
                    selectWeaponItem = aiCharacterManager._controlInventory.leftHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.leftHandWeaponIndex];
            
                }
                else
                {
                    aiCharacterManager._controlInventory.leftHandWeaponIndex += 1;
                    if (aiCharacterManager._controlInventory.leftHandWeaponIndex is < 0 or > 2)
                    {
                        aiCharacterManager._controlInventory.leftHandWeaponIndex = 0;
                    }
                    selectWeaponItem = aiCharacterManager._controlInventory.leftHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.leftHandWeaponIndex];
                }
                break;
            case 2: // IF 2 UNARMED -> SWITCH BETWEEN 1 UNARMED AND THE ONLY WEAPON ITEM
                if (aiCharacterManager._controlInventory.leftHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.leftHandWeaponIndex].ID !=
                    ItemDatabase.Instance.UnarmedWeaponItem.ID)
                {
                    selectWeaponItem = aiCharacterManager._controlInventory.leftHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.leftHandWeaponIndex];
                }
                else
                {
                    if(aiCharacterManager._controlInventory.currentLeftHandWeaponItem.ID !=
                       ItemDatabase.Instance.UnarmedWeaponItem.ID)
                    {
                        selectWeaponItem = aiCharacterManager._controlInventory.leftHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.leftHandWeaponIndex];
                    }
                    else
                    {
                        aiCharacterManager._controlInventory.leftHandWeaponIndex += 1;
                        if (aiCharacterManager._controlInventory.leftHandWeaponIndex is < 0 or > 2)
                        {
                            aiCharacterManager._controlInventory.leftHandWeaponIndex = 0;
                        }
                        selectWeaponItem = aiCharacterManager._controlInventory.leftHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.leftHandWeaponIndex];
                    }
                }
                break;
        }

        if (selectWeaponItem != null) aiCharacterManager._controlInventory.currentLeftHandWeaponItem = selectWeaponItem;
        LoadLeftHandWeapon(); 
    }
    public void SwitchRightHandWeapon()
    {
        // CAN ONLY SWITCH ON THESE CONDITION
        if (aiCharacterManager.isDoingAction || aiCharacterManager.isJumping)return;
        
        aiCharacterManager._controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Switch_Right_Hip_Weapon_01, 
                                                            false, true, true, true);
        WeaponItem selectWeaponItem = null;

        // CHECK FOR UNARMED WEAPON ITEM COUNT ON LEFT HAND
        aiCharacterManager._controlInventory.rightHandWeaponUnarmedCount = 0;
        foreach (var w in aiCharacterManager._controlInventory.rightHandWeaponItemQuickSlots)
        {
            if (w.ID == ItemDatabase.Instance.UnarmedWeaponItem.ID)
                aiCharacterManager._controlInventory.rightHandWeaponUnarmedCount++;
        }
        
        // INCREASE INDEX
        aiCharacterManager._controlInventory.rightHandWeaponIndex += 1;
        if (aiCharacterManager._controlInventory.rightHandWeaponIndex is < 0 or > 2)
        {
            aiCharacterManager._controlInventory.rightHandWeaponIndex = 0;
        }
        
        // SWITCH WEAPON RULES
        switch (aiCharacterManager._controlInventory.rightHandWeaponUnarmedCount)
        {
            case 0: // IF NO UNARMED -> SWITCH LIKE NORMAL
                selectWeaponItem = aiCharacterManager._controlInventory.rightHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.rightHandWeaponIndex];
                break;
            case 1: // IF 1 UNARMED -> SKIP THE UNARMED
                if (aiCharacterManager._controlInventory.rightHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.rightHandWeaponIndex].ID !=
                    ItemDatabase.Instance.UnarmedWeaponItem.ID)
                {
                    selectWeaponItem = aiCharacterManager._controlInventory.rightHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.rightHandWeaponIndex];
            
                }
                else
                {
                    aiCharacterManager._controlInventory.rightHandWeaponIndex += 1;
                    if (aiCharacterManager._controlInventory.rightHandWeaponIndex is < 0 or > 2)
                    {
                        aiCharacterManager._controlInventory.rightHandWeaponIndex = 0;
                    }
                    selectWeaponItem = aiCharacterManager._controlInventory.rightHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.rightHandWeaponIndex];
                }
                break;
            case 2: // IF 2 UNARMED -> SWITCH BETWEEN 1 UNARMED AND THE ONLY WEAPON ITEM
                if (aiCharacterManager._controlInventory.rightHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.rightHandWeaponIndex].ID !=
                    ItemDatabase.Instance.UnarmedWeaponItem.ID)
                {
                    selectWeaponItem = aiCharacterManager._controlInventory.rightHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.rightHandWeaponIndex];
                }
                else
                {
                    if(aiCharacterManager._controlInventory.currentRightHandWeaponItem.ID !=
                       ItemDatabase.Instance.UnarmedWeaponItem.ID)
                    {
                        selectWeaponItem = aiCharacterManager._controlInventory.rightHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.rightHandWeaponIndex];
                    }
                    else
                    {
                        aiCharacterManager._controlInventory.rightHandWeaponIndex += 1;
                        if (aiCharacterManager._controlInventory.rightHandWeaponIndex is < 0 or > 2)
                        {
                            aiCharacterManager._controlInventory.rightHandWeaponIndex = 0;
                        }
                        selectWeaponItem = aiCharacterManager._controlInventory.rightHandWeaponItemQuickSlots[aiCharacterManager._controlInventory.rightHandWeaponIndex];
                    }
                }
                break;
        }

        if (selectWeaponItem != null) aiCharacterManager._controlInventory.currentRightHandWeaponItem = selectWeaponItem;
        LoadRightHandWeapon();  
    }
    
    #endregion

    #region Damage Colliders

    public void EnableCollider()
    {
        // if (aiCharacterManager._controlCombat.isUsingRightHand)
        // {
        //     // ENABLE RIGHT HAND COLLIDER
        //     rightHandWeaponManager._damageCollider.SetCollider(true);
        // }
        // else if(aiCharacterManager._controlCombat.isUsingLeftHand)
        // {
        //     // ENABLE LEFT HAND COLLIDER
        //     leftHandWeaponManager._damageCollider.SetCollider(true);
        // }
    }
    
    public void DisableCollider()
    {
        // if (aiCharacterManager._controlCombat.isUsingRightHand)
        // {
        //     // ENABLE RIGHT HAND COLLIDER
        //     rightHandWeaponManager._damageCollider.SetCollider(false);
        //     rightHandWeaponManager._damageCollider.ClearTargetList();
        // }
        // else if(aiCharacterManager._controlCombat.isUsingLeftHand)
        // {
        //     // ENABLE LEFT HAND COLLIDER
        //     leftHandWeaponManager._damageCollider.SetCollider(false);
        //     rightHandWeaponManager._damageCollider.ClearTargetList();
        // }
    }

    #endregion
}
