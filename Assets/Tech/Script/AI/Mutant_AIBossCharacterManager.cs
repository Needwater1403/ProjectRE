using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Mutant_AIBossCharacterManager : AIBossCharacterManager
{
    [TabGroup("Manager", "Red Demon")] 
    public Mutant_AICharacterControlCombat mutantControlCombat;
    [TabGroup("Manager", "Red Demon")] 
    public Mutant_AISoundFXManager mutantControlSoundFX;
    [TabGroup("Manager", "Red Demon")] 
    public AICharacterAutoRangeWeaponManager autoRangeWeaponManager;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ChangePhase(AIStateName stateName)
    {
        // PLAY CHANGE PHASE ANIMATION & SFX HERE (TO DO)
        switch (stateName)
        {
            case AIStateName.CombatStancePhase2:
                _controlCombatBase.isInvulnerable = false;
                WorldAIManager.Instance.canProcessState = false;
                mutantControlCombat.Stomp_01_Radius += 2;
                autoRangeWeaponManager.SwitchProjectile(1);
                _controlAnimator.ResetMovement();
                _controlAnimator.PlayActionAnimationInstantly("Stun_Start", true);
                // PLAY SFX
                break;  
            case AIStateName.CombatStancePhase3:
                _controlAnimator.PlayActionAnimationInstantly("Phase3_Start", true);
                break;
        }
        
        // SET CURRENT PHASE TO NEW COMBAT STANCE PHASE (COMBAT STANCE PHASE 2)
        currentState = GetState(_controlCombat.ChangeCombatStancePhase(stateName));
        _controlCombat.SetCombatStanceMaxRange(this);
    }
    

    private void HandelEndPhase2CutScene()
    {
        currentState = GetState(AIStateName.Pursue);
        WorldAIManager.Instance.canProcessState = true;
        _controlCombatBase.isInvulnerable = true;
    }
}
