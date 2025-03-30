using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlAnimator : MonoBehaviour
{
    private CharacterManager _characterManager;
    protected static readonly int VelocityX = Animator.StringToHash("Horizontal");
    protected static readonly int VelocityZ = Animator.StringToHash("Vertical");
    protected static readonly int IsMoving = Animator.StringToHash("IsMoving");
    
    protected virtual void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
    }

    public void UpdateAnimationMovementParameter(float veloX, float veloY)
    {
        var x = veloX;
        var y = veloY;
        if (!_characterManager._controlCombatBase.isUsingConsumable && !_characterManager._controlCombatBase.isHoldingArrow)
        {
            x = x switch
            {
                <= 0.5f and >= 0.1f  => 0.5f,
                <= 1 and > 0.5f => 1,
                >= -0.5f and < -0.1f => -0.5f,
                >= -1 and < -0.5f => -1,
                _ => 0
            };

            y = y switch
            {
                <= 0.5f and > 0 => 0.5f,
                <= 1 and > 0.5f => 1,
                >= -0.5f and < 0 => -0.5f,
                >= -1 and < -0.5f => -1,
                _ => 0
            };
        }
        else
        {
            x = x switch
            {
                <= 0.5f and >= 0.1f  => 0.5f,
                <= 1 and > 0.5f => 0.5f,
                >= -0.5f and < -0.1f => -0.5f,
                >= -1 and < -0.5f => -0.5f,
                _ => 0
            };

            y = y switch
            {
                <= 0.5f and > 0 => 0.5f,
                <= 1 and > 0.5f => 0.5f,
                >= -0.5f and < 0 => -0.5f,
                >= -1 and < -0.5f => -0.5f, 
                _ => 0
            };
        }
        _characterManager._animator.SetFloat(VelocityX, x, 0.1f, Time.deltaTime);
        _characterManager._animator.SetFloat(VelocityZ, _characterManager.canSprint? 2 : y, 0.1f, Time.deltaTime);
    }

    public virtual void PlayActionAnimation(string targetAction, bool isDoingAction, bool applyRootmotion = true,
        bool canMove = false, bool canRotate = false)
    {
        _characterManager._animator.applyRootMotion = applyRootmotion;
        _characterManager._animator.CrossFade(targetAction, 0.2f);
        _characterManager.isDoingAction = isDoingAction;
        _characterManager.canRotate = canRotate;
        _characterManager.canMove = canMove;
        _characterManager.applyRootMotion = applyRootmotion;
    }
    
    public virtual void PlayActionAnimationInstantly(string targetAction, bool isDoingAction, bool applyRootmotion = true,
        bool canMove = false, bool canRotate = false)
    {
        _characterManager._animator.applyRootMotion = applyRootmotion;
        _characterManager._animator.Play(targetAction);
        _characterManager.isDoingAction = isDoingAction;
        _characterManager.canRotate = canRotate;
        _characterManager.canMove = canMove;
        _characterManager.applyRootMotion = applyRootmotion;
    }
    public virtual void PlayAttackActionAnimation(WeaponItem weaponItem, MeleeWeaponAttackType attackType, string targetAction, bool isDoingAction, bool applyRootmotion = true,
        bool canMove = false, bool canRotate = false)
    {
        // KEEP TRACKS OF ATTACK TYPE
        _characterManager._controlCombatBase.currentMeleeWeaponAttackType = attackType;
        // KEEP TRACKS OF LAST ATTACK PERFORMED FOR COMBO
        _characterManager._controlCombatBase.previousAttackAnimationName = targetAction;
        // UPDATE ANIMATION SET TO CURRENT WEAPON ANIMATION
        if(gameObject.CompareTag(Constants.PlayerTag)) UpdateAnimatorController(weaponItem);
        
        _characterManager._animator.applyRootMotion = applyRootmotion;
        _characterManager._animator.CrossFade(targetAction, 0.2f);
        _characterManager.isDoingAction = isDoingAction;
        _characterManager.canRotate = canRotate;
        _characterManager.canMove = canMove;
        _characterManager.applyRootMotion = applyRootmotion;
        
        // IF THIS ATTACK CAN BE PARRIED ? (TO DO)
    }

    public virtual void SetMovingStatus(bool isMoving)
    {
        _characterManager.isMoving = isMoving;
        _characterManager._animator.SetBool(IsMoving, isMoving);
    }
    
    // FOR AI CHARACTERS
    public virtual void SetCombatStatus(bool isCombat)
    {
    }

    public virtual void SetVerticalMovement(float velocity)
    {
    }

    public void UpdateAnimatorController(WeaponItem weaponItem)
    {
        if(weaponItem.animatorOverrideController != null) _characterManager._animator.runtimeAnimatorController = weaponItem.animatorOverrideController;
    }
}
