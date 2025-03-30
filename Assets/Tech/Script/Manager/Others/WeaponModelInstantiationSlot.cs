using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponModelInstantiationSlot : MonoBehaviour
{
    [Title("Weapon Model Slot")]
    [EnumToggleButtons]
    [HideLabel]
    public WeaponModelSlot modelSlotType;
    [Space(8)]
    
    [Title("Weapon Model")]
    [HideLabel]
    public GameObject weaponModel;
    public void UnloadWeapon()
    {
        if(weaponModel!=null) Destroy(weaponModel);
    }

    public void LoadWeapon(GameObject _weaponModel, WeaponType _weaponType)
    {
        weaponModel = _weaponModel;
        _weaponModel.transform.parent = transform;
        _weaponModel.transform.localPosition = Vector3.zero;
        _weaponModel.transform.localRotation = Quaternion.identity;
        _weaponModel.transform.localScale = Vector3.one;
        weaponModel.SetActive(true);
    }

    public void MoveWeaponModelToSpecificSlot(GameObject _weaponModel, WeaponType _weaponType, PlayerManager _playerManager)
    {
        weaponModel = _weaponModel;
        weaponModel.transform.parent = transform;
        switch (_weaponType)
        {
            case WeaponType.Mace:
                weaponModel.transform.localPosition = new Vector3(-0.1f, 0.01f, 0.09f);
                weaponModel.transform.localRotation = Quaternion.Euler(12.15f, -58.95f, -13.54f);
                break;
            case WeaponType.Bow:
                
                break;
        }

    }
} 
