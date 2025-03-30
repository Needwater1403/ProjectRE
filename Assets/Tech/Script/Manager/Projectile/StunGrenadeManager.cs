using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StunGrenadeManager : GrenadeManager
{
    [Title("Stun Effect")] 
    public float stunDuration;

    protected virtual void OnCollisionEnter(Collision other)
    {
        AOEDamage();
        var vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(realTarget);
        Destroy(vfx, stunDuration);
        Destroy(gameObject);
    }
    
    public override void SetParams(RangedProjectileItem projectileItem, AICharacterAutoRangeWeaponManager autoRangeWeaponManager = null)
    {
        base.SetParams(projectileItem, autoRangeWeaponManager);
        if(autoRangeWeaponManager!=null) 
            stunDuration = autoRangeWeaponManager.currentProjectileData.effectTime;
    } 
    
    public override void AOEDamage()
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
            
            if(character._controlStatusEffects.CheckIfCurrentlyHaveThisTimedEffect(Constants.TimedEffect_Stun_ID))
            {
                character._controlStatusEffects.RemoveTimedEffect(Constants.TimedEffect_Stun_ID);
            }
            var effect1 = Instantiate(ConfigSOManager.Instance.GetTimedEffect(Constants.TimedEffect_Stun_ID) as Stun_TimedEffects);
            effect1.time = stunDuration;
            character._controlStatusEffects.HandleTimedEffect(effect1);
        }
    }
}
