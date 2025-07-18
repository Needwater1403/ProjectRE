using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager _characterManager;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_characterManager == null) _characterManager = animator.GetComponent<CharacterManager>();
       _characterManager.isDoingAction = false;
       _characterManager.canMove = !_characterManager.isDead && !_characterManager._controlCombatBase.isStunned;
       _characterManager.canRotate = !_characterManager.isDead && !_characterManager._controlCombatBase.isStunned;
       _characterManager.applyRootMotion = false;
       _characterManager.isJumping = false;
       _characterManager.isRolling = false;
       _characterManager.isBackStepping = false;
       _characterManager._controlCombatBase.isAttacking = false;
       _characterManager._controlCombatBase.isInvulnerable = false;
       _characterManager._controlCombatBase.isBeingRiposteOrBackStab = false;
       _characterManager._controlCombatBase.isUsingConsumable = false;
       _characterManager._controlCombatBase.isTakingFallDamage = false;
       _characterManager._controlCombatBase.DisableCombo();
       _characterManager._controlCombatBase.DisableCanBeRiposteStatus();
       _characterManager._controlCombatBase.canShoot = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
