using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Mutant_StompCollider : DamageCollider
{
    [TabGroup("Damage Collider", "Red Demon Stomp")] 
    [Title("Collider Owner")] 
    public Mutant_AIBossCharacterManager _ownerAICharacterManager;
    [TabGroup("Damage Collider", "Red Demon Stomp")] 
    public float dmgMultiplier;
    public void StompAttack()
    {
        var collider = Physics.OverlapSphere(transform.position, _ownerAICharacterManager.mutantControlCombat.Stomp_01_Radius, 
                                                    WorldUltilityManager.Instance.CharactersLayers);
        var targetDamagedList = new List<CharacterManager>();
        foreach (var c in collider)
        {
            var character = c.GetComponentInParent<CharacterManager>();
            if (character == null) continue;
            if(targetDamagedList.Contains(character)) continue;
            if(character == _ownerAICharacterManager) continue;
            if (!WorldUltilityManager.CheckIfCharacterCanDamageAnother(
                    _ownerAICharacterManager.mutantControlCombat.charactersGroup, 
                    character._controlCombatBase.charactersGroup)) continue;
            
            targetDamagedList.Add(character);
            var effect = Instantiate(ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeDamage_ID) as TakeDamage_InstantEffect);
            effect.physicalDmg = physicalDmg * dmgMultiplier;
            effect.fireDmg = fireDmg * dmgMultiplier;
            effect.lightningDmg = lightningDmg * dmgMultiplier;
            effect.poiseDmg = poiseDmg;
            
            effect.contactPoint = contactPoint;
            effect.angleHitFrom = Vector3.SignedAngle(_ownerAICharacterManager.transform.forward, character.transform.forward, Vector3.up);
            
            character._controlStatusEffects.HandleInstantEffect(effect);
        }
    }
}
