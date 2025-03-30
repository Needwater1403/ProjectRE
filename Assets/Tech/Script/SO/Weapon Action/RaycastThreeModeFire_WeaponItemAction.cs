using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Three Mode Fire Action", menuName = "Config/Weapons Action/Three Mode Fire Action")]
public class RaycastThreeModeFire_WeaponItemAction : WeaponItemAction
{
    [Title("Hit Layer")]
    [SerializeField] private LayerMask layerMask;
    public override void PerformAction(PlayerManager player, WeaponItem weaponPerformTheAction)
    {
        base.PerformAction(player, weaponPerformTheAction);
        
        // CAN NOT PERFORM ACTION UNDER THESE CONDITIONS
        if(!player.isGrounded) return;
        if(!player._controlCombat.isAiming) return;
        player._controlCombat.isAttacking = true;
        
        // PERFORM NORMAL ATTACK ACTION
        PerformShootAction(player, weaponPerformTheAction);
    }

    private void PerformShootAction(PlayerManager player, WeaponItem weaponPerformTheAction)
    {
        HandleFireMode(player, weaponPerformTheAction as RangedWeaponItem);
    }
    
    private void HandleShootRaycast(PlayerManager player, RangedWeaponItem weaponPerformTheAction)
    {
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
        
        // PLAY SFX
        player._controlSoundFXBase.PlaySFX(weaponPerformTheAction.AttackSFX.GetRandom(),.45f);
        
        // SHOOT RAYCAST
        RaycastHit hit;
        if(Physics.Raycast(PlayerCamera.Instance._camera.transform.position, weaponPerformTheAction.weaponType == WeaponType.Shotgun? 
                                GetRandomSpread(weaponPerformTheAction.spreadOffset):PlayerCamera.Instance._camera.transform.forward, out hit, 100,layerMask))
        {
            switch (hit.transform.gameObject.layer)
            {
                case 3 or 7:
                    Instantiate(weaponPerformTheAction.hitTerrainVFX, hit.point, Quaternion.LookRotation(hit.normal));
                    break;
                case  9:
                    Instantiate(weaponPerformTheAction.hitZombieVFX, hit.point, Quaternion.LookRotation(hit.normal));
                    // APPLY DAMAGE
                    var target = hit.transform.GetComponentInParent<CharacterManager>();
                    var effect =
                        Instantiate(
                            ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeDamage_ID) as
                                TakeDamage_InstantEffect);
                    effect.attackerCharacterManager = player;
                    effect.attackerWeaponType = weaponPerformTheAction.weaponType;
                    effect.physicalDmg = player._controlInventory.currentAmmoItem.physicalDamage;
                    effect.fireDmg = player._controlInventory.currentAmmoItem.fireDamage;
                    effect.lightningDmg = player._controlInventory.currentAmmoItem.lightningDamage;
                    effect.poiseDmg = player._controlInventory.currentAmmoItem.poiseDamage;

                    effect.contactPoint = hit.point;
                    effect.angleHitFrom =
                        Vector3.SignedAngle(player.transform.forward, target.transform.forward, Vector3.up);
                    target._controlStatusEffects.HandleInstantEffect(effect);
                    Debug.Log(hit.transform.name);
                    break;
            }
            player._controlInventory.currentAmmoAmountInMag--;
            player._controlInventory.ammoAmountInMagList[player._controlInventory.rightHandWeaponIndex]--;
            PlayerUIManager.Instance.hubManager.SetAmmoText(player._controlInventory.currentAmmoAmountInMag, player._controlInventory.currentAmmoItem.amount);
        }
    }
    
    private void HandleFireMode(PlayerManager player, RangedWeaponItem weaponPerformTheAction)
    {
        if(weaponPerformTheAction == null) return;
        if(!player._controlCombat.canShoot) return;
        switch (weaponPerformTheAction.fireMode)
        {
            case FireMode.Single:
                for (int i = 0; i < weaponPerformTheAction.bulletPerShot; i++)
                {
                    HandleShootRaycast(player, weaponPerformTheAction);
                }
                break;
            case FireMode.Burst:
                player._controlCombat.HandleBurstModeCoroutine(IHandleBurstMode(player, weaponPerformTheAction));
                break;
            case FireMode.Auto:
                player._controlCombat.HandleAutoModeCoroutine(IHandleAutoMode(player, weaponPerformTheAction));
                break;
        }
    }

    private IEnumerator IHandleAutoMode(PlayerManager player, RangedWeaponItem weaponPerformTheAction)
    {
        while (true)
        {
            HandleShootRaycast(player, weaponPerformTheAction);
            yield return new WaitForSeconds(1/weaponPerformTheAction.fireRate);
        }
    }
    
    private IEnumerator IHandleBurstMode(PlayerManager player, RangedWeaponItem weaponPerformTheAction)
    {
        // PLAY SHOOT ANIMATION
        player._controlAnimator.PlayActionAnimation("Shoot",true, false, true, true);
        
        // PLAY WEAPON ANIMATION (TO DO)
        
        // PLAY WEAPON SFX
        player._controlEquipment.rightHandWeaponManager.PlayWeaponVFX();
        
        // HANDLE BURST MODE
        int burstCount = 0;
        while (burstCount < 3)
        {
            burstCount ++;
            // PLAY SFX
            player._controlSoundFXBase.PlaySFX(weaponPerformTheAction.AttackSFX.GetRandom(),.45f);
        
            // SHOOT RAYCAST
            RaycastHit hit;
            if(Physics.Raycast(PlayerCamera.Instance._camera.transform.position, PlayerCamera.Instance._camera.transform.forward, out hit, layerMask))
            {
                switch (hit.transform.gameObject.layer)
                {
                    case 3 or 7:
                        // INIT VFX
                        Instantiate(weaponPerformTheAction.hitTerrainVFX, hit.point, Quaternion.LookRotation(hit.normal));
                        break;
                    case 9:
                        // INIT VFX
                        Instantiate(weaponPerformTheAction.hitZombieVFX, hit.point, Quaternion.LookRotation(hit.normal));
                        
                        // APPLY DAMAGE
                        var target = hit.transform.GetComponentInParent<CharacterManager>();
                        var effect =
                            Instantiate(
                                ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeDamage_ID) as
                                    TakeDamage_InstantEffect);
                        effect.attackerCharacterManager = player;
                        effect.attackerWeaponType = weaponPerformTheAction.weaponType;
                        effect.physicalDmg = player._controlInventory.currentAmmoItem.physicalDamage;
                        effect.fireDmg = player._controlInventory.currentAmmoItem.fireDamage;
                        effect.lightningDmg = player._controlInventory.currentAmmoItem.lightningDamage;
                        effect.poiseDmg = player._controlInventory.currentAmmoItem.poiseDamage;

                        effect.contactPoint = hit.point;
                        effect.angleHitFrom =
                            Vector3.SignedAngle(player.transform.forward, target.transform.forward, Vector3.up);
                        target._controlStatusEffects.HandleInstantEffect(effect);
                        break;
                }
            }
            yield return new WaitForSeconds(1/weaponPerformTheAction.burstFireRate);
        }
    }
    
    private Vector3 GetRandomSpread(float spreadOffset)
    {
        var pos = PlayerCamera.Instance._camera.transform.position + PlayerCamera.Instance._camera.transform.forward * 100;
        pos = new Vector3(pos.x + Random.Range(-spreadOffset, spreadOffset), pos.y + Random.Range(-spreadOffset, spreadOffset), pos.z + Random.Range(-spreadOffset, spreadOffset));
        return pos;
    }
}

