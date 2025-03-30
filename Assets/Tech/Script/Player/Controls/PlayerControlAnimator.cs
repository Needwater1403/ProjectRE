using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerControlAnimator : CharacterControlAnimator
{
    private PlayerManager _playerManager;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllAnimation()
    {
        if (_playerManager.isLockedOn && !_playerManager.canSprint)
        {
            UpdateAnimationMovementParameter(InputManager.Instance.movementInputValue.x, InputManager.Instance.movementInputValue.y);
            return;
        }
        if (_playerManager._controlCombat.isAiming)
        {
            UpdateAnimationMovementParameter(0, InputManager.Instance.movementInputValue.y);
            return;
        }
        UpdateAnimationMovementParameter(0,InputManager.Instance.moveAmount);
    }

    private void OnAnimatorMove()
    {
        if (_playerManager.applyRootMotion)
        {
            var velocity = _playerManager._animator.deltaPosition;
            _playerManager._characterController.Move(velocity);
            _playerManager.transform.rotation *= _playerManager._animator.deltaRotation;
        }
    }
}
