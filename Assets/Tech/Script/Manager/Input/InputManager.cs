using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] private PlayerManager _player;
    [Title("PLAYER AND CAMERA MOVEMENT INPUT")]
    public Vector2 movementInputValue;
    public Vector2 lookInputValue;
    public float moveAmount;
    
    [Title("PLAYER MOVEMENT INPUT")]
    public bool dodgeInputValue;
    public bool sprintInputValue;
    public bool jumpInputValue;
    
    [Title("PLAYER COMBAT INPUT")]
    public bool switchWeaponInputValue;
    public bool rightLightAttackInputValue;
    
    [Title("PLAYER LOCK ON INPUT")]
    public bool lockOnInputValue;
    public bool lockOnSwitchLeftInputValue;
    public bool lockOnSwitchRightInputValue;
    
    [Title("INTERACT WITH OBJECTS INPUT")]
    public bool interactInputValue;
    public bool useItemInputValue;
    
    [Title("QUEUE INPUT")] 
    [SerializeField] private float queueInputTimer = 0.3f; 
    [SerializeField] private float queueInputCountDown;
    [Space] 
    public bool queueInputIsActive = false;
    public bool queueRightLightAttackInputValue = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    
    private void Update()
    {
        HandleAllQueueInput();
    }

    #region LOCOMOTION

    public void OnMove(InputAction.CallbackContext _context)
    {
        movementInputValue  = _context.ReadValue<Vector2>();
        moveAmount = Mathf.Clamp01(Mathf.Abs(movementInputValue.x) + Mathf.Abs((movementInputValue.y)));
        
        //CHANGE BETWEEN WALK AND RUN ANIMATION
        switch (moveAmount)
        {
            case > 0:
                moveAmount = 0.5f;
                break;
            // DO OTHER CASE IF YOU WANT MOVE INSTEAD OF WALK ONLY
        }

        _player._controlAnimator.SetMovingStatus(moveAmount != 0);
    }
    
    public void OnLook(InputAction.CallbackContext _context)
    {
        lookInputValue  = _context.ReadValue<Vector2>();
    }
    
    public void OnDodge(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                dodgeInputValue = true;
                break;
        }
    }
    
    public void OnSprint(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Started:
                sprintInputValue = true;
                break;
            case InputActionPhase.Canceled:
                sprintInputValue = false;
                break;
        }
    }

    #endregion

    #region SWITCH ITEM
    
    
    public void OnSwitchRightWeapon(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                switchWeaponInputValue = true;
                break;
        }
    }
    
    
    #endregion

    #region ATTACK
    
    public void OnAim(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                _player._controlCombat.SetAimingStatus(true);
                break;
            case InputActionPhase.Canceled:
                if(_player._controlCombatBase.isAiming) _player._controlCombat.SetAimingStatus(false);
                break;
        }
    }
    
    public void OnReload(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                _player._controlCombat.HandleReload();
                break;
        }
    }
    
    public void OnAttack(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                rightLightAttackInputValue = true;
                break;
            case InputActionPhase.Canceled:
                _player._controlCombat.isBurstOrAutoMode = false;
                break;
        }
    }
    
    #endregion

    #region LOCK ON

    public void OnLockOn(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                lockOnInputValue = true;
                Debug.Log("LOCK ON");
                break;
        }
    }
    
    public void OnLockOnSwitchLeft(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                lockOnSwitchLeftInputValue = true;
                break;
        }
    }
    
    public void OnLockOnSwitchRight(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                lockOnSwitchRightInputValue = true;
                break;
        }
    }

    #endregion

    #region QUEUE

    public void OnQueue_MeleeAttack(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                QueueInput(ref queueRightLightAttackInputValue);
                break;
        }
    }

    private void QueueInput(ref bool queueInput)
    {
        queueRightLightAttackInputValue = false;

        if (_player.isDoingAction || _player.isJumping)
        {
            queueInput = true;
            queueInputCountDown = queueInputTimer;
            queueInputIsActive = true;
        }
    }

    private void ProcessQueueInput()
    {
        if(_player.isDead || !_player._controlCombat.isUsingMeleeWeapon) return;
        if (queueRightLightAttackInputValue) rightLightAttackInputValue = true;
    }
    
    private void HandleAllQueueInput()
    {
        if (queueInputIsActive)
        {
            if (queueInputCountDown > 0)
            {
                queueInputCountDown -= Time.deltaTime;
                ProcessQueueInput();
            }
            else
            {
                // RESET
                queueInputIsActive = false;
                queueRightLightAttackInputValue = false;
                rightLightAttackInputValue = false;
                queueInputCountDown = 0;
            }
        }
    }

    #endregion

    #region INTERACT

    public void OnInteractWithObject(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Started:
                interactInputValue = true;
                break;
            case InputActionPhase.Performed:
                interactInputValue = false;
                break;
            case InputActionPhase.Canceled:
                interactInputValue = false;
                break;
        }
    }
    
    public void OnUseItem(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                useItemInputValue = true;
                break;
        }
    }
    #endregion

    #region SELECT MENU

    public void OnSelectMenu(InputAction.CallbackContext _context)
    {
        switch (_context.phase)
        {
            case InputActionPhase.Performed:
                PlayerUIManager.Instance.SetPauseMenu(true);
                break;
        }
    }

    #endregion
}
