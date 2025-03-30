using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AIBossCharacterManager : AICharacterManager
{
    [TabGroup("Manager","AI Boss")]
    [Title("Boss Info")]
    public int bossID;
    [TabGroup("Manager","AI Boss")]
    public string bossName;
    [TabGroup("Manager","AI Boss")]
    public string bossSlainPopUpText;
    [TabGroup("Manager","AI Boss")]
    public AudioClip bossIntroTrack;
    [TabGroup("Manager","AI Boss")]
    public AudioClip bossLoopTrack;
    
    [TabGroup("Manager","AI Boss")]
    [Title("Flags")]
    public bool isDefeated = false;
    [TabGroup("Manager","AI Boss")]
    public bool isAwakened = false;
    [TabGroup("Manager","AI Boss")]
    public bool isInBossEvent = false;
    
    [TabGroup("Manager","AI Boss")]
    [Title("Animation name")]
    [SerializeField] private string sleepAnimation;
    [TabGroup("Manager","AI Boss")]
    [SerializeField] private string awakenAnimation;
    
    protected override void Awake()
    {
        base.Awake();
        currentState = GetState(AIStateName.Idle);
    }

    public void SetBossEventStatus(bool _isInBossEvent)
    {
        isInBossEvent = _isInBossEvent;
        if (isInBossEvent) WorldSoundFXManager.Instance.PlayBossSoundTrack(bossIntroTrack, bossLoopTrack);
        else WorldSoundFXManager.Instance.StopBossSoundTrack(true);
        PlayerUIManager.Instance.hubManager.SetBossUIStatus(this, isInBossEvent);
    }
    
    public override void OnSpawn()
    {
        if (!isAwakened)
        {
            if(sleepAnimation != "") _controlAnimator.PlayActionAnimation(sleepAnimation, true);
        }
    }
    
    [HorizontalGroup("Manager/AI Boss/Buttons")]
    [Button("Awake Boss")]
    public void AwakeBoss()
    {
        if (!isAwakened)
        {
            if(awakenAnimation != "") _controlAnimator.PlayActionAnimation(awakenAnimation, true);
        }
        isAwakened = true;
        //currentState = GetState(AIStateName.Idle);// or Awake
        if(WorldSaveGameManager.Instance != null)
        {
            if (!WorldSaveGameManager.Instance.currentCharacterSaveData.bossesAwakenedStatus.ContainsKey(bossID))
            {
                WorldSaveGameManager.Instance.currentCharacterSaveData.bossesAwakenedStatus.Add(bossID, true);
            }
            else
            {
                WorldSaveGameManager.Instance.currentCharacterSaveData.bossesAwakenedStatus.Remove(bossID);
                WorldSaveGameManager.Instance.currentCharacterSaveData.bossesAwakenedStatus.Add(bossID, true);
            }

            WorldSaveGameManager.Instance.SaveGame();
        }
    }
    
    [HorizontalGroup("Manager/AI Boss/Buttons")]
    [Button("Defeat Boss")]
    public void DefeatBoss()
    {
        // HANDLE BOSS DEATH EVENT
        StartCoroutine(IEventDeath());
    }
    
    public override IEnumerator IEventDeath(bool manuallySelectDeathAnimation = false)
    {
        //SET BOSS STATUS
        isDefeated = true;
        SetBossEventStatus(false);
        
        // SHOW BOSS SLAIN POP UP
        PlayerUIManager.Instance.popUpsManager.ShowBossSlainPopUp(bossSlainPopUpText);
        
        return base.IEventDeath(manuallySelectDeathAnimation);
    }
}
