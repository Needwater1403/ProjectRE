using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AICharacterInventoryManager : CharacterInventoryManager
{
    public WeaponItem currentRightHandWeaponItem;
    public WeaponItem currentLeftHandWeaponItem;
    public WeaponItem currentTwoHandWeaponItem;
    
    [Title("Quick Slots")] 
    public WeaponItem[] rightHandWeaponItemQuickSlots = new WeaponItem[3];
    [HideInInspector] public int rightHandWeaponIndex;
    [HideInInspector] public int rightHandWeaponUnarmedCount;
    public WeaponItem[] leftHandWeaponItemQuickSlots = new WeaponItem[3];
    [HideInInspector] public int leftHandWeaponIndex;
    [HideInInspector] public int leftHandWeaponUnarmedCount;
}
