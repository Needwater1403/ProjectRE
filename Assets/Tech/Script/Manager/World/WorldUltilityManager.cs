using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUltilityManager : MonoBehaviour
{
    public static WorldUltilityManager Instance;
    [SerializeField] private LayerMask charactersLayers;
    [SerializeField] private LayerMask environmentLayers;

    public LayerMask CharactersLayers => charactersLayers;
    public LayerMask EnvironmentLayers => environmentLayers;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public static bool CheckIfCharacterCanDamageAnother(CharactersGroup charDoingDmg, CharactersGroup charTakingDmg)
    {
        var b = charDoingDmg switch
        {
            CharactersGroup.Enemy => charTakingDmg switch
            {
                CharactersGroup.Enemy => false,
                CharactersGroup.PlayerSide => true,
                _ => throw new ArgumentOutOfRangeException(nameof(charTakingDmg), charTakingDmg, null)
            },
            CharactersGroup.PlayerSide => charTakingDmg switch
            {
                CharactersGroup.Enemy => true,
                CharactersGroup.PlayerSide => false,
                _ => throw new ArgumentOutOfRangeException(nameof(charTakingDmg), charTakingDmg, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(charDoingDmg), charDoingDmg, null)
        };
        return b;
    }
    
    public float GetAngleOfTarget(Transform targetTf, Vector3 targetDir)
    {
        targetDir.y = 0;
        var angle = Vector3.Angle(targetTf.forward, targetDir);
        var cross = Vector3.Cross(targetTf.forward, targetDir);
        if (cross.y < 0) angle *= -1;
        return angle;
    }

    public DamageIntensity GetDamageIntensityBaseOnPoiseDamage(float poiseDamage)
    {
        var damageIntensity = DamageIntensity.Ping;
        // DAGGERS / LIGHT ATTACKS
        if (poiseDamage >= 10)
        {
            damageIntensity = DamageIntensity.Light;
        }
        // STANDARD WEAPONS / MEDIUM ATTACKS
        if (poiseDamage >= 30)
        {
            damageIntensity = DamageIntensity.Medium;
        }
        // GREAT WEAPON / HEAVY ATTACKS
        if (poiseDamage >= 70)
        {
            damageIntensity = DamageIntensity.Heavy;
        }
        // ULTRA WEAPON / COLOSSAL ATTACKS
        if (poiseDamage >= 120)
        {
            damageIntensity = DamageIntensity.Colossal;
        }

        return damageIntensity;
    }

    public Vector3 GetRipostePositionBaseOnWeaponType(WeaponType weaponType)
    {
        var defaultPos = new Vector3(-0.06f, 0, 0.756f);
        switch (weaponType)
        {
            case WeaponType.Mace:
                break;
            case WeaponType.Knuckle:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
        }

        return defaultPos;
    }
}
