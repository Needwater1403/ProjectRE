using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Fire Projectile Action", menuName = "Config/Weapons Action/Fire Projectile Action")]
public class FireProjectile_WeaponItemAction : WeaponItemAction
{
    [Title("Hit Layer")] [SerializeField] private LayerMask layerMask;

    #region Player

    public override void PerformAction(PlayerManager player, WeaponItem weaponPerformTheAction)
    {
        base.PerformAction(player, weaponPerformTheAction);

        // CAN NOT PERFORM ACTION UNDER THESE CONDITIONS
        if (!player.isGrounded) return;
        if (!player._controlCombat.isAiming) return;
        player._controlCombat.isAttacking = true;

        // PERFORM NORMAL ATTACK ACTION
        PerformShootAction(player, weaponPerformTheAction);
    }

    private void PerformShootAction(PlayerManager player, WeaponItem weaponPerformTheAction)
    {
        HandleFireProjectile(player, weaponPerformTheAction as RangedWeaponItem);
    }

    private void HandleFireProjectile(PlayerManager player, RangedWeaponItem weaponPerformTheAction)
    {
        if(weaponPerformTheAction == null) return;
        if( player._controlInventory.currentAmmoAmountInMag <= 0)
        {
            player._controlSoundFXBase.PlaySFX(weaponPerformTheAction.emptySFX.GetRandom(),.45f);
            return;
        }
        
        // PLAY SHOOT ANIMATION 
        player._controlAnimator.PlayActionAnimation("Shoot",true, false, true, true);
        
        // PLAY WEAPON ANIMATION (TO DO)
        
        // PLAY WEAPON SFX
        player._controlEquipment.rightHandWeaponManager.PlayWeaponVFX();
        
        switch (weaponPerformTheAction.ammoType)
        {
            case AmmoType.GrenadeRounds:
                // PLAY SFX
                player._controlSoundFXBase.PlaySFX(weaponPerformTheAction.AttackSFX.GetRandom(),.45f);
                
                // INIT GRENADE
                var grenadeInitTf =  player._controlEquipment.rightHandWeaponManager.projectileInitTf;
                var grenade = Instantiate(ItemDatabase.Instance.grenadeItem.releaseModel, grenadeInitTf);
                grenade.transform.localPosition = Vector3.zero;
                grenade.transform.localRotation = Quaternion.identity;
                grenade.transform.parent = null;
                
                var grenadeManager = grenade.GetComponent<GrenadeManager>();
                grenadeManager._ownerCharacterManager = player;
                
                // SET DIRECTION
                grenadeManager.target = player._controlCombat.aimTransform;
                
                // IGNORE COLLIDER
                var characterColliders = player.GetComponentsInChildren<Collider>();
                var collidersProjectileIgnore = characterColliders.ToList();
                foreach (var collider in collidersProjectileIgnore)
                {
                    Physics.IgnoreCollision(grenadeManager.collider, collider, true);
                }
                
                // SET PARAMS (VELOCITY & DAMAGE)
                grenadeManager.SetParams(ItemDatabase.Instance.grenadeItem);
                
                break;
            case AmmoType.Arrow:
                // PLAY SFX
                player._controlSoundFXBase.PlaySFX(weaponPerformTheAction.AttackSFX.GetRandom(),.45f);
                
                // INIT ARROW
                var arrowInitTf =  player._controlEquipment.rightHandWeaponManager.projectileInitTf;
                
                var aimDir = PlayerCamera.Instance._camera.transform.forward;
                // SET DIRECTION
                if (Physics.Raycast(PlayerCamera.Instance._camera.transform.position,
                        PlayerCamera.Instance._camera.transform.forward, out var hit1, 100))
                {
                    aimDir = (player._controlCombat.aimTransform.position - arrowInitTf.position).normalized;
                }
                var arrow = Instantiate(ItemDatabase.Instance.arrowItem.releaseModel, arrowInitTf.position, Quaternion.LookRotation(aimDir, Vector3.up));
                // arrow.transform.localPosition = Vector3.zero;
                // arrow.transform.localRotation = Quaternion.identity;
                // arrow.transform.parent = null;
                
                var arrowManager = arrow.GetComponent<ArrowManager>();
                arrowManager._ownerCharacterManager = player;
                
                // IGNORE COLLIDER
                var characterColliders1 = player.GetComponentsInChildren<Collider>();
                var collidersProjectileIgnore1 = characterColliders1.ToList();
                foreach (var collider in collidersProjectileIgnore1)
                {
                    Physics.IgnoreCollision(arrowManager.collider, collider, true);
                }
                
                // SET PARAMS (VELOCITY & DAMAGE)
                arrowManager.SetParams(ItemDatabase.Instance.arrowItem);
                break;
        }
        
        player._controlInventory.currentAmmoAmountInMag--;
        player._controlInventory.ammoAmountInMagList[player._controlInventory.rightHandWeaponIndex]--;
        PlayerUIManager.Instance.hubManager.SetAmmoText(player._controlInventory.currentAmmoAmountInMag, player._controlInventory.currentAmmoItem.amount);
    }

    #endregion

    #region AI

    public override void PerformAIAction(AICharacterAutoRangeWeaponManager aiCharacterRangeWeaponManager, WeaponItem weaponPerformTheAction)
    {
        base.PerformAIAction(aiCharacterRangeWeaponManager, weaponPerformTheAction);
        
        // HANDLE RELOAD
        if(aiCharacterRangeWeaponManager.currentAmmoAmountInMag <= 0)
        {
            aiCharacterRangeWeaponManager.HandleReload();
            return;
        };
        
        // PERFORM NORMAL ATTACK ACTION
        PerformAIShootAction(aiCharacterRangeWeaponManager, weaponPerformTheAction);
    }

    private void PerformAIShootAction(AICharacterAutoRangeWeaponManager aiCharacterRangeWeaponManager, WeaponItem weaponPerformTheAction)
    {
        HandleAIFireProjectile(aiCharacterRangeWeaponManager, weaponPerformTheAction as RangedWeaponItem);
    }

    private void HandleAIFireProjectile(AICharacterAutoRangeWeaponManager aiCharacterRangeWeaponManager, RangedWeaponItem weaponPerformTheAction)
    {
        if(weaponPerformTheAction == null) return;
        
        // PLAY WEAPON SFX
        aiCharacterRangeWeaponManager.weaponManager.PlayWeaponVFX();
        
        switch (weaponPerformTheAction.ammoType)
        {
            case AmmoType.GrenadeRounds:
                // PLAY SFX
                aiCharacterRangeWeaponManager.AICharacterManager._controlSoundFXBase.PlaySFX(weaponPerformTheAction.AttackSFX.GetRandom(),.45f);
                
                // INIT GRENADE
                var grenadeInitTf = aiCharacterRangeWeaponManager.weaponManager.projectileInitTf;
                var grenade = Instantiate(aiCharacterRangeWeaponManager.currentProjectileData.rangedProjectileItem.releaseModel, grenadeInitTf);
                grenade.transform.localPosition = Vector3.zero;
                grenade.transform.localRotation = Quaternion.identity;
                grenade.transform.parent = null;
                
                var grenadeManager = grenade.GetComponent<GrenadeManager>();
                grenadeManager._ownerCharacterManager = aiCharacterRangeWeaponManager.AICharacterManager;
                
                // SET DIRECTION
                grenadeManager.target = aiCharacterRangeWeaponManager.AICharacterManager._controlCombat.target.transform;
                
                // IGNORE COLLIDER
                var characterColliders = aiCharacterRangeWeaponManager.AICharacterManager.GetComponentsInChildren<Collider>();
                var collidersProjectileIgnore = characterColliders.ToList();
                foreach (var collider in collidersProjectileIgnore)
                {
                    Physics.IgnoreCollision(grenadeManager.collider, collider, true);
                }
                
                // SET PARAMS (VELOCITY & DAMAGE)
                grenadeManager.SetParams(aiCharacterRangeWeaponManager.currentProjectileData.rangedProjectileItem, aiCharacterRangeWeaponManager);
                break;
            case AmmoType.Arrow:
                // PLAY SFX
                aiCharacterRangeWeaponManager.AICharacterManager._controlSoundFXBase.PlaySFX(weaponPerformTheAction.AttackSFX.GetRandom(),.45f);
                break;
        }
        
        aiCharacterRangeWeaponManager.currentAmmoAmountInMag--;
    }

    #endregion
}