using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    [Title("References")] 
    public CharacterManager _ownerCharacterManager;
    public Rigidbody rigidbody;
    public Collider collider;
    public Transform target;
    
    [Title("Damage")]
    public int physicalDmg;
    public int fireDmg;
    public int lightningDmg;
    public int poiseDmg;
    
    [Title("Flags")] 
    public bool hasPenetrated;
    private Vector3 contactPoint;
    private List<CharacterManager> targetDamagedList = new List<CharacterManager>();
    

    private void Update()
    {
        if(target != null) transform.LookAt(target.transform);
        if(rigidbody != null)
        {
            var velocity = rigidbody.velocity;
            rigidbody.velocity = transform.forward + velocity;
        }
    }
    
    public void SetParams(RangedProjectileItem projectileItem)
    {
        physicalDmg = projectileItem.physicalDamage;
        fireDmg = projectileItem.fireDamage;
        lightningDmg = projectileItem.lightningDamage;
        poiseDmg = projectileItem.poiseDamage;
        
        rigidbody.mass = projectileItem.mass;
        var forwardVec = transform.forward * ItemDatabase.Instance.grenadeItem.forwardVelocity;
        rigidbody.velocity = forwardVec;
    }
    protected void OnCollisionEnter(Collision other)
    {
        PenetrateIntoObject(other);
        var target = other.transform.gameObject.GetComponentInParent<CharacterManager>();
        
        if(target == null) return;
        if (!WorldUltilityManager.CheckIfCharacterCanDamageAnother(_ownerCharacterManager._controlCombatBase.charactersGroup,
                target ._controlCombatBase.charactersGroup)) return;

        var contactCollider = other.gameObject.GetComponent<Collider>();
        if (contactCollider != null) contactPoint = contactCollider.ClosestPointOnBounds(transform.position);
        if(!WorldUltilityManager.CheckIfCharacterCanDamageAnother(_ownerCharacterManager._controlCombatBase.charactersGroup, 
                                                                target._controlCombatBase.charactersGroup)) return;
        ApplyDamage(target);
    }
    
    private void ApplyDamage(CharacterManager target)
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
        collider.enabled = false;
        
        Destroy(this);
        Destroy(gameObject, 15);
    }
}
