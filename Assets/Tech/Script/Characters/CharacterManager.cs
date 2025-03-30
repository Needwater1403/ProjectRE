using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmazingAssets.AdvancedDissolve;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterManager : MonoBehaviour
{
    [TabGroup("Manager","Base")]
    [Title("Preferences")] 
    public CharacterController _characterController;
    [TabGroup("Manager","Base")]
    public Animator _animator;
    [TabGroup("Manager","Base")]
    public Rigidbody _rigidbody;
    [Space]
    [TabGroup("Manager","Base")]
    public List<Renderer> _rendererList;
    [TabGroup("Manager","Base")]
    public List<Material> _materialList;
    [Space]

    [TabGroup("Manager","Base")]
    public CharacterStatManager _controlStatsBase;
    [TabGroup("Manager","Base")]
    public CharacterStatusEffectsManager _controlStatusEffects;
    [HideInInspector] public CharacterControlAnimator _controlAnimatorBase;
    [HideInInspector] public CharacterControlCombat _controlCombatBase;
    [HideInInspector] public CharacterSoundFXManager _controlSoundFXBase;
    [TabGroup("Manager","Base")]
    [Title("Status")] 
    [OnValueChanged("SetIsDeadStatus")]
    public bool isDead;
    
    [Title("Flags")] 
    [TabGroup("Manager","Base")]
    public bool isDoingAction = false;
    [TabGroup("Manager","Base")]
    
    public bool canRotate = true;
    [TabGroup("Manager","Base")]
    public bool canMove = true;
    [TabGroup("Manager","Base")]
    public bool canSprint = false;
    [TabGroup("Manager","Base")]
    public bool applyRootMotion = false;
    [TabGroup("Manager","Base")]
    public bool isJumping = false;
    [TabGroup("Manager","Base")]
    public bool isRolling = false;
    [TabGroup("Manager","Base")]
    public bool isBackStepping = false;
    [TabGroup("Manager","Base")]
    public bool isLockedOn = false;
    [TabGroup("Manager","Base")]
    public bool isGrounded = true;
    [TabGroup("Manager","Base")]
    public bool isMoving = false;
    [TabGroup("Manager","Base")]
    public bool isCombat = false;

    private static readonly int IsDead = Animator.StringToHash("IsDead");

    protected virtual void Awake()
    {
        //DontDestroyOnLoad(this);
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _controlAnimatorBase = GetComponent<CharacterControlAnimator>();
        _controlCombatBase = GetComponent<CharacterControlCombat>();
        _controlSoundFXBase = GetComponent<CharacterSoundFXManager>();
    }

    protected virtual void Start()
    {
        IgnoreOwnColliders();
        foreach (var renderer in _rendererList)
        {
            _materialList.Add(renderer.material);
        }
    }
    
    protected virtual void Update()
    {

    }
    protected virtual void LateUpdate()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual IEnumerator IEventDeath(bool manuallySelectDeathAnimation = false)
    {
        _controlStatsBase.currentHealth = 0;
        SetIsDeadStatus(true);
        _controlStatusEffects.RemoveAllTimedEffect();
        
        // RESET FLAGS 
        if (!manuallySelectDeathAnimation)
        {
            _controlAnimatorBase.PlayActionAnimation(Constants.PlayerAnimation_Death_01, true);
            yield return new WaitForSeconds(3f);
        }
        else yield return new WaitForSeconds(6.2f);
                
        // DROP ITEM 
        DropItems();
        
        // DISABLE CHARACTER
        var clip = 0f;
        while (clip < 1f)
        {
            clip += Time.deltaTime;
            foreach (var _material in _materialList)
            {
                AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(_material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }
    
    public virtual void Revive()
    {
            
    }

    protected virtual void IgnoreOwnColliders()
    {
        var characterDamageCollider = GetComponent<Collider>();
        var damageableCharacterCollider = GetComponentsInChildren<Collider>();
        var ignoreColliders = damageableCharacterCollider.ToList();
        ignoreColliders.Add(characterDamageCollider);
        foreach (var c in ignoreColliders)
        {
            foreach (var c1 in ignoreColliders)
            {
                Physics.IgnoreCollision(c,c1);
            }
        }
    }

    public void SetIsDeadStatus(bool status)
    {
        isDead = status;
        _animator.speed = 1;
        _animator.SetBool(IsDead, isDead);
        if (!isDead)
        {
            canMove = false;
            canRotate = false;
        }
    }
    
    protected virtual void DropItems()
    {
            
    }
}
