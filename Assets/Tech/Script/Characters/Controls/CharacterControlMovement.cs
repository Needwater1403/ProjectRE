using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterControlMovement : MonoBehaviour
{
    protected CharacterManager characterManager;
    protected ConfigMovementSO configMovement;

    [Title("Ground Check & Jumping")] 
    protected Vector3 yVelocity;
    protected bool fallingVelocityIsSet = false;
    protected float inAirTimer = 0;
    [SerializeField] private LayerMask groundLayer;
    private static readonly int InAirTimer = Animator.StringToHash("InAirTimer");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
     
    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }
    
    protected virtual void Start()
    {
        configMovement = ConfigSOManager.Instance.PlayerMovementConfigSO;
    }

    protected virtual void Update()
    {
        HandleGroundCheck();
        if (characterManager.isGrounded)
        {
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityIsSet = false;
                yVelocity.y = configMovement.groundYVelocity;
            }
        }
        else
        {
            if (!characterManager.isJumping && !fallingVelocityIsSet)
            {
                fallingVelocityIsSet = true;
                yVelocity.y = configMovement.fallStartVelocity;
            }

            inAirTimer += Time.deltaTime;
            if(characterManager._animator != null) characterManager._animator.SetFloat(InAirTimer, inAirTimer);
            yVelocity.y += configMovement.gravity * Time.deltaTime;
            
            // SET FALL DAMAGE MULTIPLIER
            characterManager._controlCombatBase.fallDamageMultiplier = inAirTimer;
        }
        characterManager._characterController.Move(yVelocity * Time.deltaTime);
    }
    
    private void HandleGroundCheck()
    {
        characterManager.isGrounded = Physics.CheckSphere(characterManager.transform.position, configMovement.groundCheckSphereRadius, groundLayer);
        if(characterManager._animator != null) characterManager._animator.SetBool(IsGrounded, characterManager.isGrounded);
    }

    #region Animation Event

    public void EnableRotate()
    {
        characterManager.canRotate = true;
    }
    
    public void DisableRotate()
    {
        characterManager.canRotate = false;
    }

    
    #endregion
}
