using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Mutant_AISoundFXManager : CharacterSoundFXManager
{
    [TabGroup("SFX","Mutant")]
    [SerializeField] private Mutant_AIBossCharacterManager mutantManager;
    
    [TabGroup("SFX","Mutant")]
    [Title("Stomp SFX")] 
    [SerializeField] protected RarityRandomList<AudioClip> stompVFX;
    
    [TabGroup("SFX","Mutant")]
    [Title("Axe Swing SFX")] 
    [SerializeField] protected RarityRandomList<AudioClip> axeSwingVFX;
    
    public void PlayStompSFX(float volume = 1)
    {
        PlaySFX(stompVFX.GetRandom(),volume);
    }
    
    public override void PlayAttackSFX(float volume)
    {
        PlaySFX(axeSwingVFX.GetRandom(), volume);
    }
}
