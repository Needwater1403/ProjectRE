using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Actions/Attack")]
[InlineEditor]
public class AIAttackAction : ScriptableObject
{
    [Title("Animation")] 
    [SerializeField] private string animationName;
    
    [Title("Combo Action")] 
    public AIAttackAction comboAction;
    [Range(0.0f, 100.0f)]
    public float chanceToDoCombo = 25;
    
    [Title("Attack Type")] 
    [SerializeField] private MeleeWeaponAttackType attackType;

    [Title("Flags")]
    [SerializeField] private bool isUsingRightHand;
    [SerializeField] private bool isUsingLeftHand;
    [SerializeField] private bool isUsingDuelWield;
    [Space]
    [SerializeField] private bool canBeParried = true;
    [Title("Action Values")] 
    public int weight = 50;

    public float recoveryTime = 1.4f;
    public float minAttackAngle = -35f;
    public float maxAttackAngle = 35f;
    public float minAttackDistance = 0;
    public float maxAttackDistance = 2;

    public void PerformAction(AICharacterManager aiCharacterManager)
    {
        // FOR SIMPLE ENEMY USE MOSTLY ANIMATION BASED
        aiCharacterManager._controlAnimatorBase.PlayActionAnimation(animationName, true);
    }
}
