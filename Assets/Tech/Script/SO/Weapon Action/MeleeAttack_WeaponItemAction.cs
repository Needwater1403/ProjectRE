using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Melee Weapon Action", menuName = "Config/Weapons Action/Melee Attack Action")]
public class MeleeAttack_WeaponItemAction : WeaponItemAction
{
    [Title("Normal Attack Action")]
    [SerializeField] private string animation_MeleeAttack_01 = "Main_Light_Attack_01";
    [SerializeField] private string animation_MeleeAttack_02 = "Main_Light_Attack_02";
    
    public override void PerformAction(PlayerManager player, WeaponItem weaponPerformTheAction)
    {
        base.PerformAction(player, weaponPerformTheAction);
        
        // CAN NOT PERFORM ACTION UNDER THESE CONDITIONS
        if(player._controlStatsBase.currentStamina <= 0) return;
        if(!player.isGrounded) return;
        player._controlCombat.isAttacking = true;
        
        // TRY TO PERFORM CRITICAL ATTACK
        player._controlCombat.HandleCriticalAttack();
        
        // PERFORM NORMAL ATTACK ACTION
        PerformMeleeAttackAction(player, weaponPerformTheAction);
    }

    private void PerformMeleeAttackAction(PlayerManager player, WeaponItem weaponPerformTheAction)
    {
        // MAIN HAND LIGHT ATTACK
        if (player._controlCombat.canDoRightCombo && player.isDoingAction)
        {
            player._controlCombat.canDoRightCombo = false;
            if (player._controlCombat.previousAttackAnimationName == animation_MeleeAttack_01)
            {
                player._controlAnimator.PlayAttackActionAnimation(weaponPerformTheAction, MeleeWeaponAttackType.LightAttack02, animation_MeleeAttack_02, true);
            }
            else
            {
                player._controlAnimator.PlayAttackActionAnimation(weaponPerformTheAction, MeleeWeaponAttackType.LightAttack01, animation_MeleeAttack_01, true);
            }
        }
        else if(!player.isDoingAction && player.isGrounded)
        {
            player._controlAnimator.PlayAttackActionAnimation(weaponPerformTheAction, MeleeWeaponAttackType.LightAttack01, animation_MeleeAttack_01, true);
        }
    }
}
