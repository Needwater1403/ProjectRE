using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerSoundFXManager : CharacterSoundFXManager
{
    [TabGroup("SFX", "Player")] 
    [SerializeField] private PlayerManager _playerManager;
    
    [TabGroup("SFX", "Player")]
    [Title("Heal SFX")] 
    [SerializeField] protected AudioClip healSFX;
    [TabGroup("SFX", "Player")]
    
    public void PlayHealingSFX(float volume)
    {
        PlaySFX(healSFX, volume);
    }

}

