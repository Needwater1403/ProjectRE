using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    public CharacterManager _ownerCharacterManager;
    [Title("Movement")]
    public Rigidbody rigidbody;
    public Collider collider;
    public Transform target;
    public Transform realTargetPrefab;
    protected GameObject realTarget;
    [Title("Damage")]
    public int physicalDmg;
    public int fireDmg;
    public int lightningDmg;
    public int poiseDmg;
    [Space]
    public float radius;
    public GameObject explosionVFX;

    protected void Update()
    {
        if(target != null && realTarget != null) transform.LookAt(realTarget.transform);
        if(rigidbody != null)
        {
            var velocity = rigidbody.velocity;
            rigidbody.velocity = transform.forward + velocity;
        }
    }

    public virtual void SetParams(RangedProjectileItem projectileItem, AICharacterAutoRangeWeaponManager autoRangeWeaponManager = null)
    {
        physicalDmg = projectileItem.physicalDamage;
        fireDmg = projectileItem.fireDamage;
        lightningDmg = projectileItem.lightningDamage;
        poiseDmg = projectileItem.poiseDamage;
        radius = projectileItem.explosionRadius;
        rigidbody.mass = projectileItem.mass;
        
        var upwardVec = transform.up * projectileItem.upwardVelocity;
        var forwardVec = transform.forward * projectileItem.forwardVelocity;
        var totalVec = upwardVec + forwardVec;
        rigidbody.velocity = totalVec;
        realTarget = Instantiate(realTargetPrefab.gameObject, target.position + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f),0,UnityEngine.Random.Range(-0.2f, 0.2f)), target.rotation);
    }
    protected virtual void OnCollisionEnter(Collision other)
    {
        AOEDamage();
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(realTarget);
        Destroy(gameObject);
    }
    
    public virtual void AOEDamage()
    {
        var collider = Physics.OverlapSphere(transform.position, radius, WorldUltilityManager.Instance.CharactersLayers);
        var targetDamagedList = new List<CharacterManager>();
        foreach (var c in collider)
        {
            var character = c.GetComponentInParent<CharacterManager>();
            if (character == null) continue;
            if(targetDamagedList.Contains(character)) continue;
            if(character == _ownerCharacterManager) continue;
     
            if (!WorldUltilityManager.CheckIfCharacterCanDamageAnother(
                    _ownerCharacterManager._controlCombatBase.charactersGroup, 
                    character._controlCombatBase.charactersGroup)) continue;
            
            targetDamagedList.Add(character);
            var effect = Instantiate(ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeDamage_ID) as TakeDamage_InstantEffect);
            effect.physicalDmg = physicalDmg;
            effect.fireDmg = fireDmg;
            effect.lightningDmg = lightningDmg;
            effect.poiseDmg = poiseDmg;
            
            effect.angleHitFrom = Vector3.SignedAngle(transform.forward, character.transform.forward, Vector3.up);
            
            character._controlStatusEffects.HandleInstantEffect(effect);
        }
    }
}
