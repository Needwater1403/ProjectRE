using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Constants
{
    //==================TAG=================
    public const string PlayerTag = "Player";
    public const string BossTag = "Boss";
    
    //=============PLAYER ANIMATION NAME=============
    public const string PlayerAnimation_Roll_Forward_01 = "Roll_F_01";
    public const string PlayerAnimation_Backstep_01 = "BackStep_01"; 
    public const string PlayerAnimation_Jump_Start_01 = "Jump_01_start";
    public const string PlayerAnimation_Death_01 = "Death_01";
    public const string PlayerAnimation_Fall_Death_01 = "Fall_Death_01";
    public const string PlayerAnimation_Switch_Left_Hip_Weapon_01 = "Switch_Left_Hip_Weapon_01";
    public const string PlayerAnimation_Switch_Right_Hip_Weapon_01 = "Switch_Right_Hip_Weapon_01";
    public const string PlayerAnimation_Hit_Forward_Medium_01 = "Hit_Forward_Medium_01";
    public const string PlayerAnimation_Hit_Left_Medium_01 = "Hit_Left_Medium_01";
    public const string PlayerAnimation_Hit_Right_Medium_01 = "Hit_Right_Medium_01";
    public const string PlayerAnimation_Hit_Back_Medium_01 = "Hit_Back_Medium_01";
    public const string PlayerAnimation_Hit_Forward_Ping_01 = "Hit_Forward_Ping_01";
    public const string PlayerAnimation_Hit_Left_Ping_01 = "Hit_Left_Ping_01";
    public const string PlayerAnimation_Hit_Right_Ping_01 = "Hit_Right_Ping_01";
    public const string PlayerAnimation_Hit_Back_Ping_01 = "Hit_Back_Ping_01";
    public const string PlayerAnimation_Stance_Break_01 = "Stance_Break_01";
    public const string PlayerAnimation_Riposte_01 = "Riposte_01";
    public const string PlayerAnimation_Riposte_Target_01 = "Riposte_Target_01";
    public const string PlayerAnimation_Healing_01 = "Healing_01";
    public const string PlayerAnimation_Healing_Empty_01 = "Healing_Empty_01";
    
    //=============STATIC EFFECT ID=============
    
    
    //=============INSTANT EFFECT ID=============
    public const string InstantEffect_TakeDamage_ID = "IE00";
    public const string InstantEffect_TakeBlockDamage_ID = "IE01";
    public const string InstantEffect_TakeCriticalDamage_ID = "IE02";
    public const string InstantEffect_CurseInfected_ID = "IE03";
    
    //=============TIMED EFFECT ID=============
    public const string TimedEffect_Stun_ID = "TE00";
    
    //=============ITEM ID=============
    public const int Item_Weapon_ID = 1000;
    public const int Item_Projectile_ID = 2000;
    public const int Item_Ammo_ID = 3000;
    public const int Item_Consumable_ID = 4000;
    public const int Item_KeyItems_ID = 5000;
}
