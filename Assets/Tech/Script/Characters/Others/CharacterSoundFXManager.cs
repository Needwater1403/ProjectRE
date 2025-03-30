using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

public class CharacterSoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [TabGroup("SFX", "Base")]
    [Title("Grunt SFX")] 
    [SerializeField] protected RarityRandomList<AudioClip> characterGruntVFX; 
    
    [TabGroup("SFX", "Base")]
    [Title("Attack SFX")] 
    [SerializeField] protected RarityRandomList<AudioClip> characterAttackVFX;

    public void PlayRollSFX()
    {
        _audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollSFX,.1f);
    }
    
    public void PlaySFX(AudioClip sfx, float volume = 1f, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        _audioSource.PlayOneShot(sfx, volume);
        _audioSource.pitch = 1;
        if (randomizePitch)
        {
            _audioSource.pitch += UnityEngine.Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public virtual void PlayGruntSFX(float volume)
    {
        PlaySFX(characterGruntVFX.GetRandom(), volume);
    }
    
    public virtual void PlayAttackSFX(float volume)
    {
        PlaySFX(characterAttackVFX.GetRandom(), volume);
    }
    
    public virtual void PlayStanceBreakSFX(float volume)
    {
        _audioSource.PlayOneShot(WorldSoundFXManager.Instance.stanceBreakSFX, volume);
    }
    
    public virtual void PlayCriticalHitSFX(float volume)
    {
        _audioSource.PlayOneShot(WorldSoundFXManager.Instance.criticalHitSFX, volume);
    }
    
}
