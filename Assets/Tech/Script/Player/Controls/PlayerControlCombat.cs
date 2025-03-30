using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.Screen;

public class PlayerControlCombat : CharacterControlCombat
{
    private PlayerManager _playerManager;
    [TabGroup("Combat", "Player")]
    public WeaponItem currentWeaponPerformingAction;
    [TabGroup("Combat", "Player")]
    [Title("Rig")] 
    [SerializeField] private Rig aimRig;
    [Title("Projectile")]
    [TabGroup("Combat", "Player")]
    public RangedProjectileItem currentProjectileBeingUsed;

    [TabGroup("Combat", "Player")]
    public Vector3 projectileDir;
    [Title("Layers")]
    [TabGroup("Combat", "Player")]
    [SerializeField] private LayerMask canHitLayers;
    [Title("Aim Transform")]
    [TabGroup("Combat", "Player")]
    public Transform aimTransform;
    
    private float aimRigWeight;
    public Coroutine rapidFireCoroutineHandle;
    protected override void Awake()
    {
        base.Awake();
        _playerManager = GetComponent<PlayerManager>();
    }

    public void HandleCombat()
    {
        if(_playerManager.isDead) return;
        HandleHandInput();
        HandleLockOn();
        HandleLockOnSwitch();
        HandleUseConsumableItem();
        HandleAimRig();
    }

    private void HandleAimRig()
    {
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20);
        var screenCenter = new Vector2(width / 2, height / 2);
        var ray = PlayerCamera.Instance._camera.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, canHitLayers))
        {
            aimTransform.position = hit.point;
        }
    }
    
    [FormerlySerializedAs("isRapidFire")] public bool isBurstOrAutoMode;
    #region Hand Input
    
    private void HandleHandInput()
    {
        if (InputManager.Instance.rightLightAttackInputValue)
        {
            InputManager.Instance.rightLightAttackInputValue = false;
            
            // PERFORM ACTION
            HandleRightLightAttackWeaponAction();
        }
        if(!isBurstOrAutoMode && !InputManager.Instance.rightLightAttackInputValue)
        {
            if(rapidFireCoroutineHandle != null)
            {
                StopCoroutine(rapidFireCoroutineHandle);
            }
        }
    }
    
    public void HandleRightLightAttackWeaponAction()
    {
        HandleCurrentWeaponAction(_playerManager._controlInventory.currentWeaponItem.weaponItemAction
            ,_playerManager._controlInventory.currentWeaponItem);
    }
    
    public void HandleCurrentWeaponAction(WeaponItemAction weaponAction, WeaponItem weaponItem)
    {
        weaponAction.PerformAction(_playerManager, weaponItem);
    }
    #endregion

    #region Critical Attack

    public override void HandleRiposte(CharacterManager _target)
    {
        if(_target == null) return;
        if (!_target._controlCombatBase.canBeRiposted) return;
        if (_target._controlCombatBase.isBeingRiposteOrBackStab) return;
        
        _target._controlCombatBase.isBeingRiposteOrBackStab = true;
        isInvulnerable = true;
        MeleeWeaponItem riposteWeapon;
        MeleeWeaponDamageCollider collider;

        riposteWeapon = _playerManager._controlInventory.currentWeaponItem as MeleeWeaponItem;
        collider = _playerManager._controlEquipment.rightHandWeaponManager._damageCollider;
        
        _playerManager._controlAnimator.PlayActionAnimationInstantly(Constants.PlayerAnimation_Riposte_01, true,false);
        
        // APPLY CRITICAL DAMAGE
        var effect = Instantiate(ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeCriticalDamage_ID) as TakeCriticalDamage_InstantEffect);
        effect.physicalDmg = collider.physicalDmg;
        effect.fireDmg = collider.fireDmg;
        effect.lightningDmg = collider.lightningDmg;
        
        effect.physicalDmg *= collider.riposte_Damage_Modifier;
        effect.fireDmg *= collider.riposte_Damage_Modifier;
        effect.lightningDmg *= collider.riposte_Damage_Modifier;
        
        _target._controlStatusEffects.HandleInstantEffect(effect);
        _target._controlAnimatorBase.PlayActionAnimationInstantly(Constants.PlayerAnimation_Riposte_Target_01, true);
        StartCoroutine(_target._controlCombatBase.MoveToRipostePosition(_playerManager, WorldUltilityManager.Instance.GetRipostePositionBaseOnWeaponType(riposteWeapon.weaponType)));
    }
    
    #endregion
    
    #region Lock On

    private Coroutine lockOnCoroutineHandle;

    private void HandleLockOn()
    {
        // CHECK IF TARGET IS DEAD
        if(_playerManager.isLockedOn)
        {
            if (target == null) return;
            if (target.isDead)
            {
                _playerManager.isLockedOn = false;
                target = null;
                // FIND NEW TARGET
                if(lockOnCoroutineHandle != null) StopCoroutine(lockOnCoroutineHandle);
                lockOnCoroutineHandle = StartCoroutine(PlayerCamera.Instance.FindNewLockOnTarget());
            }
        }

        // LOCK OFF
        if (InputManager.Instance.lockOnInputValue && _playerManager.isLockedOn)
        {
            InputManager.Instance.lockOnInputValue = false;
            // HANDLE LOCK OFF FUNC
            _playerManager.isLockedOn = false;
            PlayerCamera.Instance.ClearLockOnTargetList();
            target = null;
        }
        
        // LOCK ON
        if (InputManager.Instance.lockOnInputValue && !_playerManager.isLockedOn)
        {
            InputManager.Instance.lockOnInputValue = false;
            // RETURN IF IS USING RANGE WEAPON
            if(!isUsingMeleeWeapon) return;
            
            // HANDLE LOCK ON FUNC
            PlayerCamera.Instance.HandleLocatingLockOnTarget();
            if (PlayerCamera.Instance.currentLockOnTarget != null)
            {
                _playerManager.isLockedOn = true;
                SetTarget(PlayerCamera.Instance.currentLockOnTarget);
            }
        }
    }

    private void HandleLockOnSwitch()
    {
        if (InputManager.Instance.lockOnSwitchLeftInputValue)
        {
            InputManager.Instance.lockOnSwitchLeftInputValue = false;
            if (_playerManager.isLockedOn)
            {
                PlayerCamera.Instance.HandleLocatingLockOnTarget();
                if (PlayerCamera.Instance.leftLockOnTarget != null)
                {
                    SetTarget(PlayerCamera.Instance.leftLockOnTarget);
                }
            }
        }
        
        if (InputManager.Instance.lockOnSwitchRightInputValue)
        {
            InputManager.Instance.lockOnSwitchRightInputValue = false;
            if (_playerManager.isLockedOn)
            {
                PlayerCamera.Instance.HandleLocatingLockOnTarget();
                if (PlayerCamera.Instance.rightLockOnTarget != null)
                {
                    SetTarget(PlayerCamera.Instance.rightLockOnTarget);
                }
            }
        }
    }

    public override void SetTarget(CharacterManager newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget;
            previousTarget = newTarget;
        }
        else
        {
            target = null;
        }
    }
    
    #endregion
    
    #region Animation Event
    public override void DrainStaminaBasedOnAttackTypeEvent()
    {
        if(currentWeaponPerformingAction == null || !isUsingMeleeWeapon) return;
        float staminaCost = 0;
        var meleeWeapon = currentWeaponPerformingAction as MeleeWeaponItem;
        if (meleeWeapon == null) return;
        switch (currentMeleeWeaponAttackType)
        {
            case MeleeWeaponAttackType.LightAttack01:
                staminaCost = meleeWeapon.baseStaminaCost *
                              meleeWeapon.lightAttack01_StaminaCost_Multiplier;
                _playerManager._controlStatsBase.SetStamina(-staminaCost);
                break;
            case MeleeWeaponAttackType.LightAttack02:
                staminaCost = meleeWeapon.baseStaminaCost *
                              meleeWeapon.lightAttack02_StaminaCost_Multiplier;
                _playerManager._controlStatsBase.SetStamina(-staminaCost);
                break;
        }
        Debug.Log($"<color=orange>{currentMeleeWeaponAttackType}</color> Stamina Cost: <color=green>{staminaCost}</color>");
    }

    public override void EnableCombo()
    {
        canDoRightCombo = true;
    }
    
    public override void DisableCombo()
    {
        canDoRightCombo = false;
    }

    #endregion

    #region Projectile

    public void HandleReleaseProjectile()
    {
        // hasArrowNotched = false;
        // if(_playerManager._controlStatusEffects.currentDrawnProjectileVFX != null) 
        //     Destroy(_playerManager._controlStatusEffects.currentDrawnProjectileVFX);
        //
        // Animator bowAnimator = GetComponentInChildren<Animator>();
        // bowAnimator.SetBool("isDrawn", false);
        // bowAnimator.Play("Bow_Fire_01");
        //
        // if(currentProjectileBeingUsed.amount <= 0) return;
        // var projectileInitTf = _playerManager._controlCombat.lockOnTransform;
        // var projectile = Instantiate(currentProjectileBeingUsed.releaseModel, projectileInitTf);
        // var damageCollider = projectile.GetComponent<RangedProjectileDamageCollider>();
        // var rigidbody = damageCollider.rigidbody;
        // damageCollider._ownerCharacterManager = _playerManager;
        //
        // var yRotationDuringFire = _playerManager.transform.localEulerAngles.y;
        // // FIRE PROJECTILE
        // if (isAiming)
        // {
        //     var ray = new Ray(lockOnTransform.position, PlayerCamera.Instance.aimDir);
        //     projectileDir = ray.GetPoint(5000);
        //     projectile.transform.LookAt(projectileDir);
        // }
        // else
        // {
        //     if (target != null)
        //     {
        //         var rot = Quaternion.LookRotation(target._controlCombatBase.lockOnTransform.position -
        //                                           projectile.transform.position);
        //         projectile.transform.rotation = rot;
        //     }
        //     else
        //     {
        //         // TEMP
        //         _playerManager.transform.rotation = Quaternion.Euler(_playerManager.transform.rotation.x,
        //             _playerManager.transform.rotation.y, _playerManager.transform.rotation.z);
        //         var rot = Quaternion.LookRotation(_playerManager.transform.forward);
        //         projectile.transform.rotation = rot;
        //     }
        // }
        // // CONSUME ITEM 
        // currentProjectileBeingUsed.ConsumeItem(_playerManager, currentProjectileSlotBeingUsed);
        //
        // // DAMAGE EQUATION
        // damageCollider.physicalDmg = 20;
        //
        // // GET ALL COLLIDERS AND IGNORE SELF
        // var characterColliders = _playerManager.GetComponentsInChildren<Collider>();
        // var collidersProjectileIgnore = characterColliders.ToList();
        // foreach (var collider in collidersProjectileIgnore)
        // {
        //     Physics.IgnoreCollision(damageCollider.collider, collider, true);
        // }
        // rigidbody.AddForce(projectile.transform.forward * currentProjectileBeingUsed.forwardVelocity);
        // projectile.transform.parent = null;
        // // PLAY SFX (TO DO)
    }
    
    [Button]
    public void SetAimingStatus(bool _isAiming)
    {
        if(isUsingMeleeWeapon) return;
        isAiming = !_playerManager.isDoingAction && _isAiming;
        _playerManager._animator.SetBool("IsAiming", isAiming);
        if(!isAiming)
        {
            aimRigWeight = 0f;
            PlayerUIManager.Instance.hubManager.HideAllCrossHair();
        }
        else
        {
            if (_playerManager.isDoingAction) return;
            aimRigWeight = 1f;
            _playerManager._controlAnimator.PlayActionAnimation("Aim",false, false, true, true);
            PlayerUIManager.Instance.hubManager.ShowCrossHair(_playerManager._controlInventory.currentWeaponItem.weaponType);
            switch (_playerManager._controlInventory.currentWeaponItem.weaponType)
            {
                case WeaponType.Pistol:
                    _playerManager.rig_hint.localPosition = new Vector3(0.015f, 0.075f, 0.1f);
                    _playerManager.rig_hint.localRotation = Quaternion.Euler(0,0,0);
                    _playerManager.rig_target.localPosition = new Vector3(0.06f, 0.044f, 0.071f);
                    _playerManager.rig_target.localRotation = Quaternion.Euler(-60f,-284.377f,-236.657f);
                    break;
                case WeaponType.Rifle:
                    _playerManager.rig_hint.localPosition = new Vector3(0.251f, 0.086f, 0.013f);
                    _playerManager.rig_hint.localRotation = Quaternion.Euler(0,0,0);
                    _playerManager.rig_target.localPosition = new Vector3(0.284f, 0.044f, -0.012f);
                    _playerManager.rig_target.localRotation = Quaternion.Euler(-36.836f,-277.463f,-243.872f);
                    break;
                case WeaponType.Launcher:
                    break;
                case WeaponType.Shotgun:
                    _playerManager.rig_hint.localPosition = new Vector3(0.2871f, 0.1741f, 0.0247f);
                    _playerManager.rig_hint.localRotation = Quaternion.Euler(0,0,0);
                    _playerManager.rig_target.localPosition = new Vector3(0.425f, 0.105f, 0.041f);
                    _playerManager.rig_target.localRotation = Quaternion.Euler(-26.977f,-290.791f,-263.634f);
                    break;
            }
            
            // PLAY SFX
            var rangedWeapon = _playerManager._controlInventory.currentWeaponItem as RangedWeaponItem;
            if(rangedWeapon != null) _playerManager._controlSoundFXBase.PlaySFX(rangedWeapon.aimFX.GetRandom(),.45f);
        }
    }
    #endregion
    
    #region Consumable

    public void HandleUseConsumableItem()
    {
        if (!InputManager.Instance.useItemInputValue) return;
        InputManager.Instance.useItemInputValue = false;
        if (isUsingConsumable) return;
        if(_playerManager._controlInventory.currentConsumable == null) return;
        
        if (PlayerManager.Instance.isLockedOn)
        {
            InputManager.Instance.lockOnInputValue = false;
            // HANDLE LOCK OFF FUNC
            PlayerManager.Instance.isLockedOn = false;
            PlayerCamera.Instance.ClearLockOnTargetList();
            PlayerManager.Instance._controlCombat.target = null;
        }
        if(isAiming) SetAimingStatus(false);
        isUsingConsumable = true;
        
        if(_playerManager._controlInventory.currentConsumable.CheckIsUsable(_playerManager))
            _playerManager._controlInventory.currentConsumable.PlayAnimation(_playerManager);
        else _playerManager._controlInventory.currentConsumable.PlayCantUseAnimation(_playerManager);
    }
    
    public void HandleConsumableItemEffect()
    {
        if (_playerManager._controlInventory.currentConsumable != null)
            _playerManager._controlInventory.currentConsumable.PerformItemAction(_playerManager);
    }
    
    #endregion
    
    public void HandleAutoModeCoroutine(IEnumerator coroutine)
    {
        isBurstOrAutoMode = true;
        rapidFireCoroutineHandle = StartCoroutine(coroutine);
    }
    public void HandleBurstModeCoroutine(IEnumerator coroutine)
    {
        isBurstOrAutoMode = true;
        rapidFireCoroutineHandle = StartCoroutine(coroutine);
    }

    public void HandleReload()
    {
        SetAimingStatus(false);
        if(_playerManager._controlInventory.currentAmmoItem.amount <= 0 ||
           _playerManager._controlInventory.currentAmmoAmountInMag >= _playerManager._controlInventory.currentAmmoCapacity) return;
        
        // IF CURRENT CAPACITY <= TOTAL AMOUNT
        if(_playerManager._controlInventory.currentAmmoCapacity <= _playerManager._controlInventory.currentAmmoItem.amount)
        {
            var usedAmount = _playerManager._controlInventory.currentAmmoCapacity -
                             _playerManager._controlInventory.currentAmmoAmountInMag;
            _playerManager._controlInventory.currentAmmoAmountInMag += usedAmount;
            _playerManager._controlInventory.ammoAmountInMagList
                [_playerManager._controlInventory.rightHandWeaponIndex] += usedAmount;
            _playerManager._controlInventory.currentAmmoItem.amount -= usedAmount;
        }
        else
        {
            // EX: CAPACITY 30 => 30/30 => FIRE => 24/30 => USED AMOUNT = 6
            var usedAmount = _playerManager._controlInventory.currentAmmoCapacity -
                             _playerManager._controlInventory.currentAmmoAmountInMag;
            
            // IF USED AMOUNT > TOTAL AMOUNT
            if(usedAmount > _playerManager._controlInventory.currentAmmoItem.amount)
            {
                _playerManager._controlInventory.currentAmmoAmountInMag +=
                    _playerManager._controlInventory.currentAmmoItem.amount;
                _playerManager._controlInventory.ammoAmountInMagList
                        [_playerManager._controlInventory.rightHandWeaponIndex] +=
                    _playerManager._controlInventory.currentAmmoItem.amount;
                _playerManager._controlInventory.currentAmmoItem.amount = 0;
            }
            // IF USED AMOUNT <= TOTAL AMOUNT
            else
            {
                _playerManager._controlInventory.currentAmmoAmountInMag += usedAmount;
                _playerManager._controlInventory.ammoAmountInMagList
                    [_playerManager._controlInventory.rightHandWeaponIndex] += usedAmount;
                _playerManager._controlInventory.currentAmmoItem.amount -= usedAmount;
            }
        }
        // PLAY SFX
        var rangedWeapon = _playerManager._controlInventory.currentWeaponItem as RangedWeaponItem;
        if(rangedWeapon != null) _playerManager._controlSoundFXBase.PlaySFX(rangedWeapon.reloadSFX.GetRandom(),.45f);
        
        // PLAY ANIMATION & SET UI
        _playerManager._controlAnimator.PlayActionAnimation("Reload", true, false, true, true);
        PlayerUIManager.Instance.hubManager.SetAmmoText(_playerManager._controlInventory.currentAmmoAmountInMag, _playerManager._controlInventory.currentAmmoItem.amount);
    }

    public void SetIsStunnedStatus(bool isEnable)
    {
        isStunned = isEnable;
        SetAimingStatus(false);
        _playerManager._animator.SetBool("IsStun", isStunned);
        if(isStunned) _playerManager._controlAnimator.PlayActionAnimation("Stun", true, false);
    }
}
