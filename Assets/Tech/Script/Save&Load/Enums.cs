using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
}

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    Empty,
}

public enum InputDevice
{
    KeyBoard_Mouse,
    Gamepad,
}
#region WEAPON

public enum WeaponModelSlot
{
    RightHandWeapon,
    Back,
    LeftHips,
    RightHips,
    Empty,
}

public enum WeaponType
{
    Mace,
    Bow,
    Pistol,
    Rifle,
    Launcher,
    Shotgun,
    Knuckle,
}

public enum FireMode
{
    Single,
    Burst,
    Auto,
}
public enum MeleeWeaponAttackType
{
    LightAttack01,
    LightAttack02,
}
public enum LootDropType
{
   WorldDrop,
   CharacterDrop,
}
#endregion
public enum CharactersGroup
{
    Enemy,
    PlayerSide,
}

public enum DamageIntensity
{
    Ping,
    Light,
    Medium,
    Heavy,
    Colossal,
}

public enum AIStateName
{
    Sleep,
    Awaken,
    Idle,
    Pursue,
    Attack,
    CombatStancePhase1,
    CombatStancePhase2,
    CombatStancePhase3,
    Strafe,
    StepBack,
}

public enum ItemType
{
    Consumable,
    Weapon,
    Ammo,
    KeyItems,
    Empty,
}

public enum DamageType
{
    Physical,
    Fire,
    Lightning,
}

public enum DamageOvertimeType
{
    Poison,
    Toxic,
}
public enum DamageHpPercentageType
{
    Bleed,
    Frostbite,
}
public enum AmmoType
{
    HandgunAmmo,
    MAGAmmo,
    ShotgunShells,
    GrenadeRounds,
    Arrow,
}

public enum AttributeType
{
    Health,
    Magic,
    Stamina,
}

public enum Area
{
    Mausoleum,
    HowlingCathedral,
    TheForgottenShrine,
}

public enum SystemTab
{
    GameOptions,
    Camera,
    Sound,
    Display,
    Controller,
    KeyboardMouse,
    Graphics,
    QuitGame,
}
public enum OptionsTab
{
    Audio,
    Video,
    Quit
}


 