using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterControlAnimator : CharacterControlAnimator
{
    [SerializeField] private AICharacterManager _AICharacterManager;
    private static readonly int IsCombat = Animator.StringToHash("IsCombat");
    private static readonly int IsStrafing = Animator.StringToHash("IsStrafing");
    [HideInInspector] public float verticalMovement;
    protected override void Awake()
    {
        base.Awake();
        if (_AICharacterManager == null) _AICharacterManager.GetComponent<AICharacterManager>();
    }
    
    public override void SetCombatStatus(bool isCombat)
    {
        _AICharacterManager.isCombat = isCombat;
        _AICharacterManager._animator.SetBool(IsCombat, isCombat);
    }

    public override void SetVerticalMovement(float velocity)
    {
        _AICharacterManager._animator.SetFloat(VelocityZ, _AICharacterManager.canSprint? 2 : velocity, 0.1f, Time.deltaTime);
    }

    public void ResetMovement()
    {
        _AICharacterManager.isMoving = false;
        _AICharacterManager._animator.SetFloat(VelocityZ, 0);
        _AICharacterManager._animator.SetFloat(VelocityX, 0);
    }
    public override void SetMovingStatus(bool isMoving)
    {
        base.SetMovingStatus(isMoving);
        if(!isMoving) SetVerticalMovement(0);
        SetVerticalMovement(verticalMovement);
    }
    
    public void SetStrafingStatus(bool isStrafing, bool isLeft = false)
    {
        _AICharacterManager.isStrafing = isStrafing;
        base.SetMovingStatus(isStrafing);
        if(isStrafing) _AICharacterManager._animator.SetFloat(VelocityX, isLeft?-0.5f:0.5f, 0.1f, Time.deltaTime);
        else _AICharacterManager._animator.SetFloat(VelocityX, 0, 0.1f, Time.deltaTime);
        _AICharacterManager._animator.SetFloat(VelocityZ, 0, 0.1f, Time.deltaTime);
    }
    
    public void SetWalkBackStatus(bool isWalkingBack)
    {
        _AICharacterManager.isWalkingBack = isWalkingBack;
        base.SetMovingStatus(isWalkingBack);
        _AICharacterManager._animator.SetFloat(VelocityX, 0, 0.1f, Time.deltaTime);
        _AICharacterManager._animator.SetFloat(VelocityZ, isWalkingBack?-0.5f:0, 0.1f, Time.deltaTime);
    }
}
