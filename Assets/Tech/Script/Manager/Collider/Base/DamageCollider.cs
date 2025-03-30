using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class DamageCollider : MonoBehaviour
{
    [TabGroup("Damage Collider", "Base")] 
    public Collider collider;
    [Title("Damage Type")] 
    [TabGroup("Damage Collider", "Base")] 
    public float physicalDmg = 0;
    [TabGroup("Damage Collider", "Base")] 
    public float fireDmg = 0;
    [TabGroup("Damage Collider", "Base")] 
    public float lightningDmg = 0;
    
    [Title("Poise")] 
    [TabGroup("Damage Collider", "Base")] 
    public float poiseDmg;
    [TabGroup("Damage Collider", "Base")] 
    public float staminaDamage;
    
    [Title("Contact Point")] 
    protected Vector3 contactPoint;
    protected List<CharacterManager> targetDamagedList = new List<CharacterManager>();

    protected virtual void Awake()
    {
        if (collider == null) collider = GetComponentInChildren<Collider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponentInParent<CharacterManager>();
        if (target != null)
        {
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            // APPLY DAMAGE
            ApplyDamage(target);
        }
    }
    
    protected virtual void ApplyDamage(CharacterManager target)
    {
        if(targetDamagedList.Contains(target)) return;
        targetDamagedList.Add(target);
        var effect = Instantiate(ConfigSOManager.Instance.GetInstantEffect(Constants.InstantEffect_TakeDamage_ID) as TakeDamage_InstantEffect);
        effect.physicalDmg = physicalDmg;
        effect.fireDmg = fireDmg;
        effect.lightningDmg = lightningDmg;
        effect.poiseDmg = poiseDmg;
        
        effect.contactPoint = contactPoint;
        target._controlStatusEffects.HandleInstantEffect(effect);
    }

    public void SetCollider(bool isEnable) 
    {
        collider.enabled = isEnable;
        if (!isEnable)
        {
            ClearTargetList();
        }
    }
    
    public void ClearTargetList()
    {
        targetDamagedList.Clear();
    }
}
