using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager Instance;

    [Title("Audio Source")] 
    [TabGroup("Audio Source","Boss")]
    [SerializeField] private AudioSource bossIntroPlayer;
    [TabGroup("Audio Source","Boss")]
    [SerializeField] private AudioSource bossLoopPlayer;
    
    [TabGroup("Audio Source","Ambient")]
    [Title("Audio Source")] 
    [SerializeField] private AudioSource ambientPlayer;
    [TabGroup("Audio Source","Ambient")]
    [Title("Audio Clip")] 
    [SerializeField] private List<AudioClip> ambientClips;
    
    [TabGroup("Audio Source","SFX")]
    [Title("Audio Source")] 
    [SerializeField] private AudioSource popUpSFXPlayer;
    
    [TabGroup("Audio Source","SFX")]
    [Title("Action SFX")]
    public AudioClip rollSFX;
    [TabGroup("Audio Source","SFX")]
    public AudioClip pickItemSFX;
    [TabGroup("Audio Source","SFX")]
    public AudioClip stanceBreakSFX;
    [TabGroup("Audio Source","SFX")]
    public AudioClip criticalHitSFX;
    [Title("Other SFX")]
    [TabGroup("Audio Source","SFX")]
    public AudioClip bossSlainSFX;
    [TabGroup("Audio Source","SFX")]
    public AudioClip youDieSFX;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        PlayAmbientSoundTrack(.4f, 2);
    }

    #region Boss
    public void PlayBossSoundTrack(AudioClip introTrack, AudioClip loopTrack)
    {
        StopAmbientSoundTrack();
        bossIntroPlayer.volume = 1;
        bossIntroPlayer.clip = introTrack;
        bossIntroPlayer.loop = false;
        bossIntroPlayer.Play();
        
        bossLoopPlayer.volume = 1;
        bossLoopPlayer.clip = loopTrack;
        bossLoopPlayer.loop = true;
        bossLoopPlayer.PlayDelayed(introTrack.length);
    }
    
    public void StopBossSoundTrack(bool isBossDefeated = false)
    {
        if(bossIntroPlayer.isPlaying)
        {
            bossLoopPlayer.Stop();
            StartCoroutine(FadeOutBossMusicIntro(2f, isBossDefeated));
        }
        else StartCoroutine(FadeOutBossMusic(2f, isBossDefeated));
    }

    #endregion
    
    #region Ambient

    private void PlayAmbientSoundTrack(float volume = .4f, float time = 7f)
    {
        ambientPlayer.clip = ambientClips[0];
        ambientPlayer.volume = 0;
        ambientPlayer.loop = true;
        ambientPlayer.Play();
        ambientPlayer.DOFade(volume, time);
        Debug.Log("<color=orange>WORLD AMBIENT AUDIO IS NOW PLAYING</color> " + ambientPlayer.clip.name);
    }
    
    public void StopAmbientSoundTrack()
    {
        StartCoroutine(FadeOutAmbient(.5f));
    }
    
    #endregion

    #region Coroutine

    private IEnumerator FadeOutBossMusicIntro(float duration, bool isBossDefeated = false)
    {
        bossIntroPlayer.DOFade(0, duration);
        if(isBossDefeated)
        {
            yield return new WaitForSeconds(duration);
            bossIntroPlayer.Stop();
            bossIntroPlayer.volume = 1;
            bossIntroPlayer.clip = bossSlainSFX;
            bossIntroPlayer.loop = false;
            bossIntroPlayer.Play();
        }
        PlayAmbientSoundTrack();
    }
    private IEnumerator FadeOutBossMusic(float duration, bool isBossDefeated = false)
    {
        bossLoopPlayer.DOFade(0, duration);
        if(isBossDefeated)
        {
            yield return new WaitForSeconds(duration);
            bossLoopPlayer.Stop();
            bossIntroPlayer.volume = 1;
            bossIntroPlayer.clip = bossSlainSFX;
            bossIntroPlayer.loop = false;
            bossIntroPlayer.Play();
        }
        PlayAmbientSoundTrack();
    }
    
    private IEnumerator FadeOutAmbient(float duration)
    {
        ambientPlayer.DOFade(0, duration);
        yield return new WaitForSeconds(duration);
        ambientPlayer.Stop();
    }
    #endregion
}
