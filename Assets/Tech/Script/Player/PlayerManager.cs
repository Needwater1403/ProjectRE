using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerManager : CharacterManager
{
    public static PlayerManager Instance;

    [TabGroup("Manager", "Player")] 
    [Title("Basic Info")]
    public Transform aimingCamFollowTf;

    [TabGroup("Manager", "Player")] 
    [Title("Rig")]
    public Transform rig_hint;
    [TabGroup("Manager", "Player")] 
    public Transform rig_target;
    
    [TabGroup("Manager", "Player")] 
    [Title("Input")]
    public PlayerInput _playerInput;

    [TabGroup("Manager","Player")]
    [Title("Controls")]
    public PlayerControlMovement _controlMovement;
    [TabGroup("Manager","Player")]
    public PlayerControlAnimator _controlAnimator;
    [TabGroup("Manager","Player")]
    public PlayerControlCombat _controlCombat;
    
    [Title("Inventory & Equipment")]
    [TabGroup("Manager","Player")]
    public PlayerEquipmentManager _controlEquipment;
    [TabGroup("Manager","Player")]
    public PlayerInventoryManager _controlInventory;
    
    [TabGroup("Manager","Player")]
    [Title("Interactive")] 
    public PlayerInteractableManager _controlInteractive;
    
    [TabGroup("Manager", "Player")] 
    [Title("SFX")]
    public PlayerSoundFXManager _controlSoundFX;

    [TabGroup("Manager", "Player")] 
    [Title("Flags")]
    public bool isFrozen;
    
    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    protected override void Start()
    {
        if(PlayerCamera.Instance != null) PlayerCamera.Instance.SetCameraBehindPlayer();
        
        base.Start();
    }
    
    protected override void Update()
    {
        if(isFrozen) return;
        base.Update();
        if (_controlMovement)
        {
            _controlMovement.HandleAllLocomotion();
        }
        if (_controlAnimator)
        {
            _controlAnimator.HandleAllAnimation();
        }
        if (_controlStatsBase)
        {
            _controlStatsBase.StaminaRegen();
        }
        if (_controlCombat)
        {
            _controlCombat.HandleCombat();
        }
        if (_controlEquipment)
        {
            _controlEquipment.HandleAllSwitch();
        }
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
        if(PlayerCamera.Instance != null) PlayerCamera.Instance.HandleCamera();
    }

    public void FootstepWalk()
    {
        // DELETE AFTER ADD THE FOOTSTEP PACKAGE
    }

    public void FootstepRun()
    {
        // DELETE AFTER ADD THE FOOTSTEP PACKAGE
    }
    public override IEnumerator IEventDeath(bool manuallySelectDeathAnimation = false)
    {
        yield return null;
         //SHOW YOU DIED POP UP IF THIS CHARACTER IS PLAYER
         PlayerUIManager.Instance.popUpsManager.ShowYouDiedPopUp();
         
         //PLAY SFX
         WorldSoundFXManager.Instance.PlayPopUpSfx(WorldSoundFXManager.Instance.youDieSFX);
         
         _controlStatsBase.currentHealth = 0;
         SetIsDeadStatus(true);
        
         // RESET FLAGS 
         if (!manuallySelectDeathAnimation && !PlayerCamera.Instance.isDeadCam)
         {
             _controlAnimatorBase.PlayActionAnimation(
                 !_controlCombat.isTakingFallDamage ? Constants.PlayerAnimation_Death_01 : Constants.PlayerAnimation_Fall_Death_01, true);
         }
        
         yield return new WaitForSeconds(5);
         _controlStatusEffects.RemoveAllTimedEffect();
        
         // DISABLE DEAD CAM
         PlayerCamera.Instance.isDeadCam = false;
        
         // RELOAD SCENE
         if(SceneLoaderManager.Instance != null)
         {
             SceneLoaderManager.Instance.AttemptToReloadScene();
         }
    }
    
    #region Debug
    [TabGroup("Manager", "Player")]
    [Title("DEBUG")]
    [Button("Dead")]
    public void Dead(bool isManuallySelectDeathAnim)
    {
        StartCoroutine(IEventDeath(isManuallySelectDeathAnim));
    }
    
    [TabGroup("Manager", "Player")]
    [Button("Revive")]
    public override void Revive()
    {
        base.Revive();
        SetIsDeadStatus(false);
        isLockedOn = false;
        _controlStatsBase.SetHealth(_controlStatsBase.maxHealth); 
        _controlStatsBase.SetStamina(_controlStatsBase.maxStamina);
        _controlAnimator.PlayActionAnimation("Emptyy", false, false);
    }

    public void SetFreezeStatus(bool _isFrozen)
    {
        isFrozen = _isFrozen;
        canMove = !isFrozen;
        canRotate = !isFrozen;
    }

    [TabGroup("Manager", "Player")]
    [Button("Stun")]
    public void InflictStun()
    {
        var effect = Instantiate(ConfigSOManager.Instance.GetTimedEffect(Constants.TimedEffect_Stun_ID) as Stun_TimedEffects);
        effect.time = 5;
        _controlStatusEffects.HandleTimedEffect(effect);
    }
    #endregion
}
