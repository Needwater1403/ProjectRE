using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStun_TimedEffects", menuName = "Config/Status Effect/Timed Effects/Stun")]
public class Stun_TimedEffects : TimedEffects
{
    public override void SetUpID()
    {
        base.SetUpID();
        EffectID = Constants.TimedEffect_Stun_ID;
    }
    
    public override void ProcessEffect(CharacterManager characterManager)
    {
        base.ProcessEffect(characterManager);
        var playerManager = characterManager as PlayerManager;
        if (playerManager != null)
        {
            playerManager._controlCombat.SetIsStunnedStatus(true);
        } 
    }
    
    public override void RemoveEffect(CharacterManager characterManager)
    {
        base.RemoveEffect(characterManager);
        var playerManager = characterManager as PlayerManager;
        if (playerManager != null)
        {
            playerManager._controlCombat.SetIsStunnedStatus(false);
        }
    }
}
