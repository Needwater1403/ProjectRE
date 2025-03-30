using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

public class RangedProjectileDamageCollider : DamageCollider
{
    [TabGroup("Damage Collider", "Ranged Projectile")] 
    [Title("Collider Owner")] 
    public CharacterManager _ownerCharacterManager;
    [TabGroup("Damage Collider", "Ranged Projectile")] 
    public Rigidbody rigidbody;
    [TabGroup("Damage Collider", "Ranged Projectile")]
    public CapsuleCollider capsuleCollider;
    
    [Title("Flags")] 
    [TabGroup("Damage Collider", "Ranged Projectile")] 
    public bool hasPenetrated;
    
    private void FixedUpdate()  
    {
        if (rigidbody.velocity != Vector3.zero)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }
    }
    
    protected void OnCollisionEnter(Collision other)
    {
        PenetrateIntoObject(other);
        var target = other.transform.gameObject.GetComponent<CharacterManager>();
        
        if(target == null) return;
        if (!WorldUltilityManager.CheckIfCharacterCanDamageAnother(_ownerCharacterManager._controlCombatBase.charactersGroup,
                target ._controlCombatBase.charactersGroup)) return;

        var contactCollider = other.gameObject.GetComponent<Collider>();
        if (contactCollider != null) contactPoint = contactCollider.ClosestPointOnBounds(transform.position);
        if(!WorldUltilityManager.CheckIfCharacterCanDamageAnother(_ownerCharacterManager._controlCombatBase.charactersGroup, 
                                                                target._controlCombatBase.charactersGroup)) return;
        ApplyDamage(target);
    }
    
    protected override void ApplyDamage(CharacterManager target)
    {
        if(targetDamagedList.Contains(target))  return;
        targetDamagedList.Add(target);
        // ALWAYS INSTANTIATE NEW SCRIPTABLE OBJECT TO AVOID CHANGING THE ORIGINAL VALUE
        var effect = Instantiate(ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeDamage_ID) as TakeDamage_InstantEffect);
        effect.physicalDmg = physicalDmg;
        effect.fireDmg = fireDmg;
        effect.lightningDmg = lightningDmg;
        effect.poiseDmg = poiseDmg;
        
        effect.contactPoint = contactPoint;
        effect.angleHitFrom = Vector3.SignedAngle(_ownerCharacterManager.transform.forward, target.transform.forward, Vector3.up);
        
        // APPLY DAMAGE
        target._controlStatusEffects.HandleInstantEffect(effect);
        
        // INIT IMPACT SFX

    }

    private void PenetrateIntoObject(Collision hit)
    {
        if(hasPenetrated) return;
        hasPenetrated = true;
        
        gameObject.transform.position = hit.GetContact(0).point;
        var empty = new GameObject();
        empty.transform.parent = hit.collider.transform;
        gameObject.transform.SetParent(empty.transform, true);

        transform.position += transform.forward * (UnityEngine.Random.Range(-.25f, .1f));
        
        rigidbody.isKinematic = true;
        capsuleCollider.enabled = false;
        
        Destroy(GetComponent<RangedProjectileDamageCollider>());
        Destroy(gameObject, 15);
    }
}
