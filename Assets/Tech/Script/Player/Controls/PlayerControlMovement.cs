using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerControlMovement : CharacterControlMovement
{
    private PlayerManager _playerManager;

    #region Parameters

    [Title("MOVE & ROTATE")]
    private Vector2 moveValue;
    public Vector3 moveDir;
    private Vector3 rotationDir;

    [Title("DODGE")] 
    private Vector3 rollDir;
    
    [Title("JUMP")] 
    private Vector3 jumpDir;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _playerManager = GetComponent<PlayerManager>();
    }
    
    protected override void Start()
    {
        base.Start();
    }
    private void GetMovementInputValue()
    {
        if(InputManager.Instance != null) moveValue = InputManager.Instance.movementInputValue;
    }
    
    public void HandleAllLocomotion() //MOVEMENT BASE ON CAMERA PERSPECTIVE
    {
        // MOVEMENT
        HandleAllMovement();
       
        // ACTION
        HandleAllAction();
    }

    private void HandleAllMovement()
    {
        //======= Grounded movement handle =======
        HandleGroundMovement();
        
        //======= Aerial movement handle =======
        HandleAerialMovement();
        HandleJumpMovement();
        HandleFreeFallingMovement();
        
        //======= Rotation handle =======
        HandleRotation();
    }
    
    private void HandleAllAction()
    {
        if(InputManager.Instance == null) return;
        HandleDodge();
        HandleSprint();
        HandleJump();
    }

    protected override void Update()
    {
        if(_playerManager.isFrozen) return;
        base.Update();
    }
    private void HandleGroundMovement()
    {
        if(_playerManager.canMove || _playerManager.canRotate) GetMovementInputValue();
        if(!_playerManager.canMove) return;
        if(PlayerCamera.Instance == null) return;
        if(!_playerManager._controlCombat.isAiming)
        {
            var transform1 = PlayerCamera.Instance.transform;
            moveDir = transform1.forward * moveValue.y;
            moveDir += transform1.right * moveValue.x;
            moveDir.Normalize();
            moveDir.y = 0;
        }
        else
        {

            moveDir = transform.forward * moveValue.y;
            moveDir += transform.right * moveValue.x;
            moveDir.Normalize();
            moveDir.y = 0;
        }
        if (_playerManager.canSprint)
        {
            _playerManager._characterController.Move(moveDir * (configMovement.runSpeed * Time.deltaTime));
        }
        else
        {
            _playerManager._characterController.Move(moveDir * (configMovement.walkSpeed * Time.deltaTime));
            // if (InputManager.Instance.moveAmount <= 0.5f || _playerManager._controlCombat.isUsingConsumable 
            //     || _playerManager._controlCombat.isHoldingArrow)
            // {
            //     _playerManager._characterController.Move(moveDir * (configMovement.walkSpeed * Time.deltaTime));
            // }
            // else if (InputManager.Instance.moveAmount <= 1)
            // {
            //     _playerManager._characterController.Move(moveDir * (configMovement.runSpeed * Time.deltaTime));
            // }
        }
    }

    private void HandleAerialMovement() 
    {
        
    }

    private void HandleRotation()
    {
        if(!_playerManager.canRotate) return;
        if(PlayerCamera.Instance == null) return;
        if (_playerManager._controlCombat.isAiming)
        {
            HandleAimingRotation();
        }
        else
        {
            HandleNormalRotation();
        }
    }
    private void HandleAimingRotation()
    {
        var newDir = Quaternion.Euler(0, PlayerCamera.Instance.transform.eulerAngles.y, 0);
        var camDir = Quaternion.Slerp(transform.rotation, newDir, configMovement.rotationSpeed * Time.deltaTime);
        transform.rotation = camDir;
    }
    private void HandleNormalRotation()
    {
        if (_playerManager.isLockedOn)
        {
            if (_playerManager.canSprint || _playerManager.isRolling || _playerManager.isBackStepping)
            {
                // SPRINT ROTATION LIKE NORMALLY
                rotationDir = Vector3.zero;
                var cam = PlayerCamera.Instance._camera.transform;
                rotationDir = cam.forward * moveValue.y;
                rotationDir += cam.right * moveValue.x;
                rotationDir.Normalize();
                rotationDir.y = 0;
                if (rotationDir == Vector3.zero)
                {
                    rotationDir = transform.forward;
                }
                var newDir = Quaternion.LookRotation(rotationDir);
                var camDir = Quaternion.Slerp(transform.rotation, newDir, configMovement.rotationSpeed * Time.deltaTime);
                transform.rotation = camDir;
            }
            else
            {
                // STRAFING
                if(_playerManager._controlCombat.target == null) return;
                rotationDir = _playerManager._controlCombat.target.transform.position - transform.position;
                rotationDir.Normalize();
                rotationDir.y = 0;
                var newDir = Quaternion.LookRotation(rotationDir);
                var camDir = Quaternion.Slerp(transform.rotation, newDir, configMovement.rotationSpeed * Time.deltaTime);
                transform.rotation = camDir;
            }
        }
        else
        {
            rotationDir = Vector3.zero;
            var cam = PlayerCamera.Instance._camera.transform;
            rotationDir = cam.forward * moveValue.y;
            rotationDir += cam.right * moveValue.x;
            rotationDir.Normalize();
            rotationDir.y = 0;
            if (rotationDir == Vector3.zero)
            {
                rotationDir = transform.forward;
            }
            var newDir = Quaternion.LookRotation(rotationDir);
            var camDir = Quaternion.Slerp(transform.rotation, newDir, configMovement.rotationSpeed * Time.deltaTime);
            transform.rotation = camDir;
        }
    }
    private void HandleDodge()
    {
        // CAN NOT DODGE IN THESE CONDITIONS
        if(!InputManager.Instance.dodgeInputValue) return;
        InputManager.Instance.dodgeInputValue = false;
        
        if(_playerManager.isDoingAction) return;
        //if(_playerManager._controlStatsBase.currentStamina <= 0) return;
        
        // HANDLE ACTION
        if (InputManager.Instance.moveAmount > 0) // ROLL WHEN MOVING
        {
            rollDir = PlayerCamera.Instance._camera.transform.forward * InputManager.Instance.movementInputValue.y;
            rollDir += PlayerCamera.Instance._camera.transform.right * InputManager.Instance.movementInputValue.x;
            rollDir.y = 0;
            rollDir.Normalize();
            Quaternion playerRotation = Quaternion.LookRotation(rotationDir);
            _playerManager.transform.rotation = playerRotation;
            _playerManager._controlAnimator.PlayActionAnimation(Constants.PlayerAnimation_Roll_Forward_01, true);
            //_playerManager._controlSoundFXBase.PlayRollSFX();
            _playerManager.isRolling = true;
        }
        else // BACK STEP WHEN STANDING STILL   
        {
            _playerManager.isBackStepping = true;
            _playerManager._controlAnimator.PlayActionAnimation(Constants.PlayerAnimation_Backstep_01, true);
        }
        
        // SET STAMINA CONSUMPTION 
        _playerManager._controlStatsBase.SetStamina(-_playerManager._controlStatsBase.staminaConsumptionS0.staminaDodge);
        _playerManager._controlCombat.CancelAllAttemptAction();
    }

    private void HandleSprint()
    {
        // CAN NOT SPRINT IN THESE CONDITIONS
        if(!InputManager.Instance.sprintInputValue)
        {
            _playerManager.canSprint = false;
            return;
        }
        if (_playerManager.isDoingAction || _playerManager._controlStatsBase.currentStamina <= 0)
        {
            _playerManager.canSprint = false;
            return;
        }
        
        // HANDLE ACTION
        if (InputManager.Instance.moveAmount >= 0.5f) // SPRINT ONLY WHEN MOVING
        {
            _playerManager.canSprint = true;
            
            // SET STAMINA CONSUMPTION
            _playerManager._controlStatsBase.SetStamina(-_playerManager._controlStatsBase.staminaConsumptionS0.staminaSprint * Time.deltaTime);
        }
        else
        {
            _playerManager.canSprint = false;
        }
    }
    
    private void HandleJump()
    {
        // CAN NOT JUMP IN THESE CONDITIONS
        if(!InputManager.Instance.jumpInputValue) return;
        InputManager.Instance.jumpInputValue = false;
        if(_playerManager.isDoingAction) return;
        if(_playerManager._controlStatsBase.currentStamina <= 0) return;
        if(!_playerManager.isGrounded) return;
        
        // HANDLE ACTION (TEMP 1H WEAPON ONLY)
        _playerManager._controlAnimator.PlayActionAnimation(Constants.PlayerAnimation_Jump_Start_01, false);
        _playerManager.isJumping = true;
        _playerManager.isGrounded = false;
        
        // SET STAMINA CONSUMPTION
        _playerManager._controlStatsBase.SetStamina(-_playerManager._controlStatsBase.staminaConsumptionS0.staminaJump);
        
        jumpDir = PlayerCamera.Instance._camera.transform.forward * InputManager.Instance.movementInputValue.y;
        jumpDir += PlayerCamera.Instance._camera.transform.right * InputManager.Instance.movementInputValue.x;
        jumpDir.y = 0;
        //jumpDir.Normalize();

        if (jumpDir != Vector3.zero)
        {
            if (_playerManager.canSprint)
            {
                jumpDir *= 1f;
            }
            else switch (InputManager.Instance.moveAmount)
            {
                case >= 0.5f:
                    jumpDir *= .5f;
                    break;
                case < 0.5f:
                    jumpDir *= .3f;
                    break;
            }
        }
    }

    private void HandleJumpMovement()
    {
        if (_playerManager.isJumping)
        {
            _playerManager._characterController.Move(jumpDir * configMovement.jumpForwardSpeed * Time.deltaTime);
        }
    }

    private void HandleFreeFallingMovement() 
    {
        if (!_playerManager.isGrounded)
        {
            var fallDir = PlayerCamera.Instance.transform.forward * InputManager.Instance.movementInputValue.y;
            fallDir += PlayerCamera.Instance.transform.right * InputManager.Instance.movementInputValue.x;
            fallDir.y = 0;
            _playerManager._characterController.Move(fallDir * configMovement.fallSpeed * Time.deltaTime);
        }
    }
    public void AddJumpVelocity() // Animation Event
    {
        yVelocity.y = Mathf.Sqrt(configMovement.jumpHeight * -2 * configMovement.gravity);
    }
}
