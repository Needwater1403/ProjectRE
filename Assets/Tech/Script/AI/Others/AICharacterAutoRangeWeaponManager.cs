using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class AICharacterAutoRangeWeaponManager : MonoBehaviour
{
    public ConfigAIAutoRangedWeapon weaponData;
    public AICharacterManager AICharacterManager;
    [Title("Weapon")]
    public RangedWeaponItem rangedWeaponItem;
    public WeaponManager weaponManager;
    [Title("Ammo")] 
    public ConfigAIAutoRangedWeaponAmmoDataHolder currentAmmoData;
    public ConfigAIAutoRangedWeaponProjectileDataHolder currentProjectileData;
    // public AmmoItem ammoItem;
    // public RangedProjectileItem rangedProjectileItem;
    public int currentAmmoAmountInMag;
    [Title("Reload")]
    public float reloadTime;
    [Title("Range")]
    public float minRange;
    public float maxRange;
    [Title("Flags")] 
    public bool isUsingProjectile;
    public bool canShoot;
    public bool isReloading;
    
    private void Start()
    {
        SetWeaponDataOnStart();
    }

    private void Update()
    {
        if (AICharacterManager._controlCombat.target == null)
        {
            canShoot = false;
            return;
        }
        HandleFindTarget();
        HandleRangeWeaponAction();
    }

    public void HandleRangeWeaponAction()
    {
        if(isReloading) return;
        if(canShoot && AICharacterManager._controlCombat.target != null)
            rangedWeaponItem.weaponItemAction.PerformAIAction(this, rangedWeaponItem);
    }

    private void SetWeaponDataOnStart()
    {
        rangedWeaponItem = weaponData.rangedWeaponItem;
        if(!weaponData.ammoItemDataList.IsNullOrEmpty()) currentAmmoData = weaponData.ammoItemDataList[0];
        if(!weaponData.rangedProjectileItemDataList.IsNullOrEmpty()) currentProjectileData = weaponData.rangedProjectileItemDataList[0];
    }

    public void SwitchProjectile(int index)
    {
        currentProjectileData = weaponData.rangedProjectileItemDataList[index];
    }
    
    public void SwitchAmmo(int index)
    {
        currentAmmoData = weaponData.ammoItemDataList[index];
    }
    #region FIND TARGET

    public void HandleFindTarget()
    {
        // IF TARGET IS DEAD -> SKIP
        if (AICharacterManager._controlCombat.target.isDead)
        {
            canShoot = false;
            return;
        }
                
        //CHECK FIELD OF VIEW
        var targetDir = AICharacterManager._controlCombat.target.transform.position - AICharacterManager.transform.position;
        var angleOfTarget = Vector3.Angle(targetDir, AICharacterManager.transform.forward);

        if (angleOfTarget > AICharacterManager._controlCombat.minViewableAngle && angleOfTarget < AICharacterManager._controlCombat.maxViewableAngle)
        {
            // ADD LAYER MASK FOR ENVIRONMENT LAYERS ONLY
            RaycastHit hit;
            if (Physics.Linecast(AICharacterManager._controlCombatBase.lockOnTransform.position,
                    AICharacterManager._controlCombat.target._controlCombatBase.lockOnTransform.position, out hit,
                    WorldUltilityManager.Instance.EnvironmentLayers))
            {
                canShoot = false;
                return;
            }

            minRange = isUsingProjectile ? currentProjectileData.minRange : currentAmmoData.minRange;
            maxRange = isUsingProjectile ? currentProjectileData.maxRange : currentAmmoData.maxRange;
            if(AICharacterManager._controlCombat.distanceFromTarget < minRange || AICharacterManager._controlCombat.distanceFromTarget > maxRange)
                canShoot = false;
            else canShoot = true;
        }
        else canShoot = false;
    }

    #endregion

    #region RELOAD

    public void HandleReload()
    {
        StartCoroutine(IReload());
    }
    private IEnumerator IReload()
    {
        // HANDLE SHOOT
        isReloading = true;
        reloadTime = isUsingProjectile ? currentProjectileData.reloadTime : currentAmmoData.reloadTime;
        yield return new WaitForSeconds(reloadTime);
        currentAmmoAmountInMag = rangedWeaponItem.ammoCapacity;
        isReloading = false;
    }

    #endregion
}
