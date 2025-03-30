using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterControlCombat : MonoBehaviour
{
    private CharacterManager _characterManager;
    [Header("Character Group")] 
    [EnumToggleButtons]
    [HideLabel] 
    [TabGroup("Combat", "Base", false)]
    public CharactersGroup charactersGroup;
    
    [Title("Target")]
    [TabGroup("Combat","Base")]
    public CharacterManager target;
    protected CharacterManager previousTarget;
    
    [Title("Lock On")] 
    [TabGroup("Combat","Base")]
    public Transform lockOnTransform;
    
    [FormerlySerializedAs("currentWeaponAttackType")]
    [Title("Attack Type")]
    [TabGroup("Combat","Base")]
    public MeleeWeaponAttackType currentMeleeWeaponAttackType;
    
    [Title("Previous Attack Animation")]
    [TabGroup("Combat","Base")]
    public string previousAttackAnimationName;
    
    [Title("Previous Poise Damage Taken")]
    [TabGroup("Combat","Base")]
    public float previousPoiseDamage;
    
    [Title("Combat Flags")]
    [TabGroup("Combat","Base")]
    public bool isAttacking = false;
    
    [TabGroup("Combat","Base")]
    public bool isUsingMeleeWeapon = false;
    
    [TabGroup("Combat","Base")]
    public bool isBeingRiposteOrBackStab = false;
    
    [TabGroup("Combat","Base")]
    public bool isUsingConsumable = false;
    
    [TabGroup("Combat","Base")]
    public bool isHoldingArrow = false;
    
    [TabGroup("Combat","Base")]
    public bool isAiming = false;
    
    [TabGroup("Combat","Base")]
    public bool isTakingFallDamage = false;
    
    [TabGroup("Combat","Base")]
    public bool isStunned = false;
    [Space]
    
    [TabGroup("Combat","Base")]
    public bool canShoot = true;
    
    [TabGroup("Combat","Base")]
    public bool canDoRightCombo = false;
    
    [TabGroup("Combat","Base")]
    public bool canBeRiposted = false;
    
    [TabGroup("Combat","Base")]
    public bool hasArrowNotched = false;
    
    [Space] 
    [TabGroup("Combat","Base")]
    public bool isInvulnerable = false;
    
    [Space] 
    [Title("Critical Attack")]
    [TabGroup("Combat","Base")]
    public Transform ripostedTargetTransform;
    [TabGroup("Combat","Base")]
    public float criticalAttackCheckDistance = .7f;
    [TabGroup("Combat","Base")]
    public float criticalDmgTaken;

    [TabGroup("Combat","Base")]
    [Title("Fall Damage")] 
    [SerializeField] private float baseFallDamage = 150;
    protected virtual void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
    }

    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget;
        }
        else
        {
            target = null;
        }
    }
    
    #region Critical Attack

    public virtual void HandleCriticalAttack()
    {
        if(_characterManager.isDoingAction) return;
        if(_characterManager._controlStatsBase.currentStamina <= 0 ) return;
        var hits = Physics.RaycastAll(lockOnTransform.position,
            _characterManager.transform.TransformDirection(Vector3.forward), criticalAttackCheckDistance,
            WorldUltilityManager.Instance.CharactersLayers);

        foreach (var hit in hits)
        {
            var targetManager = hit.transform.GetComponent<CharacterManager>();
            if (targetManager == null) continue;
            if(targetManager == _characterManager) return;
            if (!WorldUltilityManager.CheckIfCharacterCanDamageAnother(_characterManager._controlCombatBase.charactersGroup,
                    targetManager._controlCombatBase.charactersGroup)) continue;
            var direction = _characterManager.transform.position - targetManager.transform.position;
            var targetViewableAngle = Vector3.SignedAngle(direction, targetManager.transform.forward, Vector3.up);
            if (targetManager._controlCombatBase.canBeRiposted)
            {
                if (targetViewableAngle is >= -60 and <= 60)
                {
                    HandleRiposte(targetManager);
                    return;
                }
            }
        }
    }
    
    public virtual void HandleRiposte(CharacterManager _target)
    {
        
    }
    
    public IEnumerator MoveToRipostePosition(CharacterManager characterManager, Vector3 ripostePos)
    {
        if (ripostedTargetTransform == null)
        {
            var newRipostedTargetObject = new GameObject("Riposted Pos")
            {
                transform =
                {
                    parent = transform,
                    position = Vector3.zero
                }
            };
            ripostedTargetTransform = newRipostedTargetObject.transform;
        }
        ripostedTargetTransform.localPosition = ripostePos; 
        var pos = ripostedTargetTransform.position;
        var timer = 0f;
        
        while (timer < .2f)
        {
            timer += Time.deltaTime;
            characterManager.transform.position = pos; 
            characterManager.transform.rotation = Quaternion.LookRotation(-transform.forward);
            yield return null;
        }
    }
    #endregion
    public virtual void DisableAllDamageColliders()
    {
        
    }

    // CANCEL MID ACTION WHEN POISE IS BROKEN
    public virtual void CancelAllAttemptAction()
    {
        return;
        _characterManager._controlStatusEffects.DestroyCurrentSpellVFX();
        _characterManager._controlStatusEffects.DestroyCurrentDrawnProjectileVFX();
    }
    
    #region Animation Event
    public virtual void EnableCombo()
    {
        
    }
    
    public virtual void DisableCombo()
    {
        
    }

    public virtual void EnableInvulnerable()
    {
        isInvulnerable = true;
    }
    
    public virtual void DisableInvulnerable()
    {
        isInvulnerable = false;
    }
    public virtual void DrainStaminaBasedOnAttackTypeEvent()
    {
        
    }
    
    public virtual void EnableCanBeRiposteStatus()
    {
        canBeRiposted = true;
    }
    
    public virtual void DisableCanBeRiposteStatus()
    {
        canBeRiposted = false;
    }
    
    public void HandleTakeFallDamage()
    {
        StartCoroutine(TakeFallDamage());
    }
    
    public void EnableCanShoot()
    {
        canShoot = true;
    }
    
    public void DisableCanShoot()
    {
        canShoot = false;
    }
    
    public virtual void ApplyCriticalDamage()
    {
        _characterManager._controlStatsBase.SetHealth(-criticalDmgTaken);
        Debug.Log($"<color=teal>{_characterManager.gameObject.name}</color> received <color=red>CRITICAL</color> <color=orange>{criticalDmgTaken}</color> damage. " +
                  $"Remaining HP: <color=red>{_characterManager._controlStatsBase.currentHealth}</color>!");
        //_characterManager._controlStatusEffects.PlayBloodSplatterVFX(lockOnTransform.position);
        _characterManager._controlSoundFXBase.PlayCriticalHitSFX(0.65f);
        
    }
    #endregion

    #region Fall Damage
    [HideInInspector] public float fallDamageMultiplier = 0;
    private IEnumerator TakeFallDamage()
    {
        _characterManager._animator.speed = 0;
        _characterManager._controlCombatBase.isTakingFallDamage = true;
        var effect = Instantiate(ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeDamage_ID) as TakeDamage_InstantEffect);
        effect.playDmgAnimation = false;
        effect.physicalDmg = baseFallDamage * (1 + fallDamageMultiplier);
        fallDamageMultiplier = 0;
        _characterManager._controlStatusEffects.HandleInstantEffect(effect);
        yield return new WaitForSeconds(.5f);
        _characterManager._animator.speed = 1;
    }

    #endregion
}
