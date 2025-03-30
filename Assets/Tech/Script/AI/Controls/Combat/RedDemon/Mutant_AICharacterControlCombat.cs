using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Mutant_AICharacterControlCombat : AICharacterControlCombat
{
    #region Params

    [TabGroup("Combat","Mutant")]
    [SerializeField] private Mutant_AIBossCharacterManager bossManager;
    [TabGroup("Combat","Mutant")]
    [Title("Colliders")]
    // PHASE 1
    [TabGroup("Combat","Mutant")]
    [SerializeField] private Mutant_Axe_DamageCollider rightAxeDamageCollider;
    [TabGroup("Combat","Mutant")]
    [SerializeField] private Mutant_Axe_DamageCollider leftAxeDamageCollider;
    [Space]
    [TabGroup("Combat","Mutant")]
    [Title("Stomp Attack")]
    [TabGroup("Combat","Mutant")]
    [SerializeField] private Mutant_StompCollider stompingDamageCollider;
    [FormerlySerializedAs("phase_01_Stomp_01_Radius")] [TabGroup("Combat","Mutant")]
    public float Stomp_01_Radius = 5f;
    
    [Title("Damage Multiplier")]
    // AXE
    [TabGroup("Combat","Mutant")]
    [SerializeField] private float Attack_01_DmgMultiplier;
    [TabGroup("Combat","Mutant")]
    [SerializeField] private float Attack_02_DmgMultiplier;
    [TabGroup("Combat","Mutant")]
    [SerializeField] private float Attack_03_DmgMultiplier;
    [TabGroup("Combat","Mutant")]
    [SerializeField] private float Attack_04_DmgMultiplier;
    [TabGroup("Combat","Mutant")]
    [SerializeField] private float Attack_05_DmgMultiplier;
    
    // STOMP
    [TabGroup("Combat","Mutant")]
    [SerializeField] private float Stomp_01_PhysicalDmgMultiplier;
    
    [TabGroup("Combat","Mutant")]
    [Title("Flags")] 
    public bool isPhase2 = false;

    [TabGroup("Combat","Mutant")]
    [Title("VFX")] 
    [SerializeField] private Transform stompVFXHolder;
    [Space]
    [TabGroup("Combat","Mutant")]
    [SerializeField] private GameObject Stomp_Attack_01_VFX;
   
    #endregion
    private void Start()
    {
        isUndead = false;
        rightAxeDamageCollider.SetCollider(false);
        leftAxeDamageCollider.SetCollider(false);
        SetDamageForStompAttack_01();
    }

    #region SET DAMAGE MMULTIPLIER ANIMATION EVENTS
    
    public void SetDamageForAttack_01()
    {
        rightAxeDamageCollider.dmgMultiplier = Attack_01_DmgMultiplier;
        leftAxeDamageCollider.dmgMultiplier = Attack_01_DmgMultiplier;
    }
    
    public void SetDamageForAttack_02()
    {
        rightAxeDamageCollider.dmgMultiplier = Attack_02_DmgMultiplier;
        leftAxeDamageCollider.dmgMultiplier = Attack_02_DmgMultiplier;
    }
    
    public void SetDamageForAttack_03()
    {
        rightAxeDamageCollider.dmgMultiplier = Attack_03_DmgMultiplier;
        leftAxeDamageCollider.dmgMultiplier = Attack_03_DmgMultiplier;
    }
    
    public void SetDamageForAttack_04()
    {
        rightAxeDamageCollider.dmgMultiplier = Attack_04_DmgMultiplier;
        leftAxeDamageCollider.dmgMultiplier = Attack_04_DmgMultiplier;
    }
    
    public void SetDamageForAttack_05()
    {
        rightAxeDamageCollider.dmgMultiplier = Attack_05_DmgMultiplier;
        leftAxeDamageCollider.dmgMultiplier = Attack_05_DmgMultiplier;
    }
    
    public void SetDamageForStompAttack_01()
    {
        stompingDamageCollider.dmgMultiplier = Stomp_01_PhysicalDmgMultiplier;
    }

    #endregion
    public override void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isDoingAction)
            return;
        
        switch (viewableAngle)
        {
            case >= 61 and <= 110:
                aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Right_90", true);
                break;
            case <= -61 and >= -110:
                aiCharacter._controlAnimatorBase.PlayActionAnimation("Turn_Left_90", true);
                break;
        }
    }
    
    #region HANDLE COLLIDERS ANIMATION EVENTS

    public void EnableRightHandCollider()
    {
        bossManager.mutantControlSoundFX.PlayAttackSFX(1f);
        rightAxeDamageCollider.SetCollider(true);
    }
    
    public void DisableRightHandCollider()
    {
        rightAxeDamageCollider.SetCollider(false);
    }
    
    public void EnableLeftHandCollider()
    {
        bossManager.mutantControlSoundFX.PlayAttackSFX(1f);
        leftAxeDamageCollider.SetCollider(true);
    }
    
    public void DisableLeftHandCollider()
    {
        leftAxeDamageCollider.SetCollider(false);
    }
    
    public void EnableStompAttack()
    {
        Instantiate(Stomp_Attack_01_VFX, stompVFXHolder.position, stompVFXHolder.rotation);
        stompingDamageCollider.StompAttack();
    }
    #endregion
    
    protected override void SetAIDamageDataOnStart(ConfigAISO data)
    {
        var aiData = data as Mutant_ConfigAISO;
        if(aiData == null) return;
        rightAxeDamageCollider.physicalDmg = aiData.axePhysicalDamage;
        rightAxeDamageCollider.fireDmg = aiData.axeFireDamage;
        rightAxeDamageCollider.lightningDmg = aiData.axeLightningDamage;
        rightAxeDamageCollider.poiseDmg = aiData.axePoiseDamage;
        rightAxeDamageCollider.staminaDamage = aiData.axeStaminaDamage;
        
        leftAxeDamageCollider.physicalDmg = aiData.axePhysicalDamage;
        leftAxeDamageCollider.fireDmg = aiData.axeFireDamage;
        leftAxeDamageCollider.lightningDmg = aiData.axeLightningDamage;
        leftAxeDamageCollider.poiseDmg = aiData.axePoiseDamage;
        leftAxeDamageCollider.staminaDamage = aiData.axeStaminaDamage;
        
        stompingDamageCollider.physicalDmg = aiData.stompPhysicalDamage;
        stompingDamageCollider.fireDmg = aiData.stompFireDamage;
        stompingDamageCollider.lightningDmg = aiData.stompLightningDamage;
        stompingDamageCollider.poiseDmg = aiData.stompPoiseDamage;
        stompingDamageCollider.staminaDamage = aiData.stompStaminaDamage;
        
        Attack_01_DmgMultiplier = aiData.attack01DmgMultiplier;
        Attack_02_DmgMultiplier = aiData.attack02DmgMultiplier;
        Attack_03_DmgMultiplier = aiData.attack03DmgMultiplier;
        Attack_04_DmgMultiplier = aiData.attack04DmgMultiplier;
        Attack_05_DmgMultiplier = aiData.attack05DmgMultiplier;
        
        Stomp_01_PhysicalDmgMultiplier = aiData.attackStompDmgMultiplier;
        Stomp_01_Radius = aiData.stompRadius;

        totalPhase = aiData.totalPhase;
    }
}
