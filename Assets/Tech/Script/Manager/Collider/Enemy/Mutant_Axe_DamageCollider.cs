using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Mutant_Axe_DamageCollider : DamageCollider
{
    [TabGroup("Damage Collider", "Red Demon Axe")] 
    [Title("Collider Owner")] 
    public AIBossCharacterManager _ownerAICharacterManager;
    [TabGroup("Damage Collider", "Red Demon Axe")] 
    public float dmgMultiplier;
    protected override void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponentInParent<CharacterManager>();
        Debug.Log("Trigger");
        if (target != null)
        {
            if(target == _ownerAICharacterManager) return;
            // CHECK FRIENDLY FIRE
            if (!WorldUltilityManager.CheckIfCharacterCanDamageAnother(_ownerAICharacterManager._controlCombatBase.charactersGroup,
                    target ._controlCombatBase.charactersGroup)) return;
            
            // CHECK IF THE TARGET IS INVULNERABLE
            if(target._controlCombatBase.isInvulnerable) return;
            
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            
            Debug.Log("Apply damage");
            // APPLY DAMAGE
            ApplyDamage(target);
        }
    }
    
    protected override void ApplyDamage(CharacterManager target)
    {
        if(targetDamagedList.Contains(target))  return;
        targetDamagedList.Add(target);
        // ALWAYS INSTANTIATE NEW SCRIPTABLE OBJECT TO AVOID CHANGING THE ORIGINAL VALUE
        var effect = Instantiate(ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeDamage_ID) as TakeDamage_InstantEffect);
        effect.physicalDmg = physicalDmg* dmgMultiplier;
        effect.fireDmg = fireDmg * dmgMultiplier;
        effect.lightningDmg = lightningDmg * dmgMultiplier;
        effect.poiseDmg = poiseDmg;
        
        effect.contactPoint = contactPoint;
        effect.angleHitFrom = Vector3.SignedAngle(_ownerAICharacterManager.transform.forward, target.transform.forward, Vector3.up);
        
        // APPLY DAMAGE
        target._controlStatusEffects.HandleInstantEffect(effect);
    }
}
