using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ConfigAISO  : ScriptableObject
{
    [TabGroup("AI Data", "Base")]
    #region Navmesh Movement
    [FoldoutGroup("AI Data/Base/Navmesh Movement")]
    [Title("Steering")]
    public float speed;
    [FoldoutGroup("AI Data/Base/Navmesh Movement")]
    public float angularSpeed;
    [FoldoutGroup("AI Data/Base/Navmesh Movement")]
    public float acceleration;
    
    #endregion

    #region Animation
    [FoldoutGroup("AI Data/Base/Animation")]
    [Title("Animation Params")]
    public float verticalMovement;
    [FoldoutGroup("AI Data/Base/Animation")]
    public AnimatorOverrideController animatorOverrideController;
    #endregion
    
    #region Combat

    [FoldoutGroup("AI Data/Base/Combat")]
    [Title("Line Of Sight")]
    public float lineOfSight = 13f;
    [FoldoutGroup("AI Data/Base/Combat")]
    public float minViewableAngle = -35f;
    [FoldoutGroup("AI Data/Base/Combat")]
    public float maxViewableAngle = 35f;

    [FoldoutGroup("AI Data/Base/Combat")]
    [Title("Pivot")] 
    public bool canPivot = false;
    
    [FoldoutGroup("AI Data/Base/Combat")]
    [Title("Stance")] 
    public bool ignoreStanceBreak;
    [FoldoutGroup("AI Data/Base/Combat")]
    public float maxStance = 150;
    [FoldoutGroup("AI Data/Base/Combat")]
    public float currentStance;
    [FoldoutGroup("AI Data/Base/Combat")]
    public float stanceRecoverySpeed = 15;
    [FoldoutGroup("AI Data/Base/Combat")]
    public float defaultStanceRecoveryTime = 15;
    
    [Title("Combat Stance Params")]
    [FoldoutGroup("AI Data/Base/Combat")]
    public float attackRotationSpeed = 25f;
    
    [FoldoutGroup("AI Data/Base/Combat")]
    public int totalPhase;
    [FoldoutGroup("AI Data/Base/Combat")]
    public int phase2CapPercentage;
    [FoldoutGroup("AI Data/Base/Combat")]
    public int phase3CapPercentage;
    
    #endregion

    #region Status

    [FoldoutGroup("AI Data/Base/Status")]
    [Title("Attributes")]
    public float maxHealth = 100f;
    [FoldoutGroup("AI Data/Base/Status")]
    public float maxStamina = 100f;
    [FoldoutGroup("AI Data/Base/Status")]
    public float poise = 100f;
    [FoldoutGroup("AI Data/Base/Status")]
    public float defaultPoiseResetTimer;
    [FoldoutGroup("AI Data/Base/Status")]
    public bool changePhaseBasedOnHp;
    
    #endregion
}
