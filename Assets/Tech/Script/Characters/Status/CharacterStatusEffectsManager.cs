using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterStatusEffectsManager : MonoBehaviour
{
    public CharacterManager _characterManager;

    [Title("Current Static Effects")]
    [HideLabel]
    [SerializeField] private List<StaticEffects> currentStaticEffectsList;
    [Title("Current Timed Effects")]
    [HideLabel]
    [SerializeField] private List<TimedEffects> currentTimedEffectsList;

    private Dictionary<string, Coroutine> timedEffectCoroutineDic;
    public virtual void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
        timedEffectCoroutineDic = new Dictionary<string, Coroutine>();
    }

    public virtual void HandleInstantEffect(InstantEffects instantEffects) // BLEED - CURSE - ...
    {
        instantEffects.ProcessEffect(_characterManager);
    }

    public virtual void HandleTimedEffect(TimedEffects effect) // POISON - ROT - ...
    {
        AddTimedEffect(effect);
    }

    private void AddTimedEffect(TimedEffects effect)
    {
        currentTimedEffectsList.Add(effect);
        if(!effect.isTick) effect.ProcessEffect(_characterManager);
        else StartCoroutine(IHandleTickTimedEffect(effect));
        
        for (var i = currentTimedEffectsList.Count - 1; i > -1; i--)
        {
            if(currentTimedEffectsList[i] == null) currentTimedEffectsList.RemoveAt(i);
        }

        var a = StartCoroutine(IRemoveTimedEffect(effect.EffectID, effect.time));
        timedEffectCoroutineDic.Add(effect.EffectID, a);
    }
    
    protected IEnumerator IHandleTickTimedEffect(TimedEffects effect)
    {
        effect.InitVFX(_characterManager);
        var tick = (int)(effect.time / effect.tickTime);
        while (tick > 0)
        {
            tick--;
            effect.ProcessEffect(_characterManager);
            yield return new WaitForSeconds(effect.tickTime);
        }
        yield return null;
    }
    protected IEnumerator IRemoveTimedEffect(string effectID, float time)
    {
        yield return new WaitForSeconds(time);
        RemoveTimedEffect(effectID);
    }
    public void RemoveTimedEffect(string effectID)
    {
        // HANDLE COROUTINE
        if (timedEffectCoroutineDic.TryGetValue(effectID, out var value))
        {
            StopCoroutine(value);
        }

        timedEffectCoroutineDic.Remove(effectID);
        foreach (var kv in timedEffectCoroutineDic.Where(kv => kv.Key == null))
        {
            timedEffectCoroutineDic.Remove(kv.Key);
        }
        
        // HANDLE REMOVE EFFECT
        for (var i = 0; i < currentTimedEffectsList.Count; i++)
        {
            if (currentTimedEffectsList[i] != null)
            {
                if (currentTimedEffectsList[i].EffectID == effectID)
                {
                    var effect = currentTimedEffectsList[i];
                    effect.RemoveEffect(_characterManager);
                    currentTimedEffectsList.RemoveAt(i);
                }
            }
        }
        
        for (var i = currentTimedEffectsList.Count - 1; i > -1; i--)
        {
            if(currentTimedEffectsList[i] == null) currentTimedEffectsList.RemoveAt(i);
        }
    }
    
    public void RemoveAllTimedEffect()
    {
        for (var i = 0; i < currentTimedEffectsList.Count; i++)
        {
            if (currentTimedEffectsList[i] != null)
            {
                var effect = currentTimedEffectsList[i];
                effect.RemoveEffect(_characterManager);
                currentTimedEffectsList.RemoveAt(i);
            }
        }
        
        for (var i = currentTimedEffectsList.Count - 1; i > -1; i--)
        {
            if(currentTimedEffectsList[i] == null) currentTimedEffectsList.RemoveAt(i);
        }
    }

    public bool CheckIfCurrentlyHaveThisTimedEffect(string effectID)
    {
        return currentTimedEffectsList.Any(te => te.EffectID == effectID);
    }
    public virtual void HandleStaticEffect(StaticEffects effect, bool isRemoved) // BUFFS / DEBUFFS FROM TALISMAN - ITEMS
    {
        if (!isRemoved) AddStaticEffect(effect);
        else RemoveStaticEffect(effect.EffectID);
    }

    private void AddStaticEffect(StaticEffects effect)
    {
        currentStaticEffectsList.Add(effect);
        effect.ProcessEffect(_characterManager);
        
        for (var i = currentStaticEffectsList.Count - 1; i > -1; i--)
        {
            if(currentStaticEffectsList[i] == null) currentStaticEffectsList.RemoveAt(i);
        }
    }

    private void RemoveStaticEffect(string effectID)
    {
        for (var i = 0; i < currentStaticEffectsList.Count; i++)
        {
            if (currentStaticEffectsList[i] != null)
            {
                if (currentStaticEffectsList[i].EffectID == effectID)
                {
                    var effect = currentStaticEffectsList[i];
                    effect.RemoveEffect(_characterManager);
                    currentStaticEffectsList.RemoveAt(i);
                }
            }
        }
        
        for (var i = currentStaticEffectsList.Count - 1; i > -1; i--)
        {
            if(currentStaticEffectsList[i] == null) currentStaticEffectsList.RemoveAt(i);
        }
    }

    public bool CheckStaticEffectInList(StaticEffects effect)
    {
        return currentStaticEffectsList.Contains(effect);
    }
    
    #region VFX
    [Space(10)]
    [Title("VFX")] 
    [SerializeField] protected GameObject vfx_BloodSplatter;
    [SerializeField] protected GameObject vfx_FlaskVFX;
    [SerializeField] protected GameObject vfx_AshenFlaskVFX;
    [SerializeField] protected GameObject vfx_Item;
    [SerializeField] protected GameObject vfx_BossAura;
    public GameObject currentSpellVFX;
    public GameObject currentDrawnProjectileVFX;
    public void PlayBloodSplatterVFX(Vector3 pos)
    {
        if (vfx_BloodSplatter != null)
        {
            var vfx = Instantiate(vfx_BloodSplatter, pos, Quaternion.identity);
        }
        else
        {
            var vfx = Instantiate(ConfigSOManager.Instance.vfx_BloodSplatter, pos, Quaternion.identity);
        }
    }
    
    public virtual void PlayItemVFX()
    {
       
    }
    
    public void DestroyCurrentSpellVFX()
    {
        if(currentSpellVFX!=null) Destroy(currentSpellVFX);
    }
    
    public void DestroyCurrentDrawnProjectileVFX()
    {
        if(currentDrawnProjectileVFX!=null) Destroy(currentDrawnProjectileVFX);
    }
    
    public void PlayBossAuraVFX()
    {
        if(vfx_BossAura == null) return;
        vfx_BossAura.SetActive(true);
        vfx_BossAura.GetComponent<ParticleSystem>().Play();
    }
    #endregion
}
