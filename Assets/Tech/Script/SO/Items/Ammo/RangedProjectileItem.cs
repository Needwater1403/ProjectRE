using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Projectile", menuName = "Items/Ranged Projectile")]
[InlineEditor]
public class RangedProjectileItem : AmmoItem
{
    [TabGroup("Item","Projectile")]
    [Title("Model")]
    public GameObject releaseModel;
    
    [TabGroup("Item","Projectile")]
    [Title("Velocity")]
    public float forwardVelocity = 2200;
    [TabGroup("Item","Projectile")]
    public float upwardVelocity = 0;
    [TabGroup("Item","Projectile")]
    public float mass = .01f;
    
    [TabGroup("Item","Projectile")]
    [Title("Radius")]
    [ShowIf("OnShowRadius")]
    public float explosionRadius = 5;
    
    private bool OnShowRadius()
    {
        return type == AmmoType.GrenadeRounds;
    }
}
