using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[System.Serializable]
public class CharacterSaveData
{
    [Title("Scene Index")] 
    public int sceneIndex = 1;
    public List<int> loadedSceneIndex = new();

    [Title("Area")] 
    public Area area;
    
    [Title("Play Time")] 
    public float playTime;
    
    [Title("Name")] 
    public string characterName;
    
    [Title("Name")] 
    public int covenantIndex;
    
    [Title("Last Area PopUp Text")] 
    public string lastAreaPopUpText;

    [Title("Character World Coordinates")] // CAN NOT USE VECTOR CAUSE JSON ONLY KNOW BASIC VARIABLE TYPE (INT FLOAT ....)
    public float xPos;
    public float yPos;
    public float zPos;
    public float xRot;
    public float yRot;
    public float zRot;
    
    [Title("Last Bonfire Rest ID")] // CAN NOT USE VECTOR CAUSE JSON ONLY KNOW BASIC VARIABLE TYPE (INT FLOAT ....)
    public int lastBonfireRestID;
    
    [Title("Camera Orientation")] // CAN NOT USE VECTOR CAUSE JSON ONLY KNOW BASIC VARIABLE TYPE (INT FLOAT ....)
    public float camHorizontalAngle;
    public float camVerticalAngle;

    [Title("Stats")] 
    public int level;
    public int vigor;
    public int endurance;
    public int mind;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int faith;
    
    [Title("Bosses")] // int = boss ID
    public SerializableDictionary<int, bool> bossesAwakenedStatus = new();
    public SerializableDictionary<int, bool> bossesDefeatedStatus = new();
    
    [Title("NPC Dialogue")]
    public SerializableDictionary<int, int> npcDialogue = new();
    
    [Title("Bonfires")] // int = bonfire ID
    public SerializableDictionary<int, bool> bonfires = new();

    [Title("World Items")] 
    public SerializableDictionary<int, bool> worldItemIsLootedStatus = new();
        
    [Title("New Area PopUp Trigger")] 
    public SerializableDictionary<int, bool> newAreaPopUpIsTriggeredStatus = new();
    
    [Title("Doors Open Status")] 
    public SerializableDictionary<int, bool> doorsOpenStatus = new();
    
    [Title("Items")] 
    public List<int> itemsID = new();
    public List<int> itemsAmount = new();
    
    [Title("Projectile")]
    public int mainArrow;
    public int subArrow;
    public int mainBolt;
    public int subBolt;
    
    [Title("Armors")]
    public int helmEquipment;
    public int chestEquipment;
    public int gauntletsEquipment;
    public int greavesEquipment;

    [Title("Weapons")]
    public int leftHandWeaponSlot_1_ID;
    public int leftHandWeaponSlot_2_ID;
    public int leftHandWeaponSlot_3_ID;
    
    public int leftHandWeaponSlot_1_PersonalID;
    public int leftHandWeaponSlot_2_PersonalID;
    public int leftHandWeaponSlot_3_PersonalID;
    
    public int rightHandWeaponSlot_1_ID;
    public int rightHandWeaponSlot_2_ID;
    public int rightHandWeaponSlot_3_ID;

    public int rightHandWeaponSlot_1_PersonalID;
    public int rightHandWeaponSlot_2_PersonalID;
    public int rightHandWeaponSlot_3_PersonalID;
    
    public int currentLeftHandWeaponIndex;
    public int currentRightHandWeaponIndex;
    
    [Title("Equipments")]
    public List<int> equipmentsID = new();
    public List<int> equipmentsPersonalID = new();
    public List<int> equipmentsLevel = new();

    [Title("Weapon Art")]
    public List<int> weaponArtsID = new();
    public List<int> weaponArtsPersonalID = new();

    [Title("Add Weapon Art To Weapon")]
    public List<int> weaponWithRemovableWeaponArtID = new();
    public List<int> weaponWithRemovableWeaponArtPersonalID = new();
    public List<int> equippedWeaponArtID = new();
    public List<int> equippedWeaponArtPersonalID = new();
    
    [Title("Spells")]
    public List<int> equippedSpellID = new();
    public int totalSpellSlot;
    public int remainingSpellSlot;
    public int spellIndex;
    
    [Title("Rings")]
    public int[] equippedRingID = new int[4];

    [Title("Consumables")] 
    public int currency;
    public int[] equippedConsumableID = new int[10];
        
    [Title("Retrieve Soul Mark")] 
    public bool isRetrieveSoulMarkExist;
    public int soulRetrieveValue;
    public float retrieveSoulMarkPosX;
    public float retrieveSoulMarkPosY;
    public float retrieveSoulMarkPosZ;
    
    [Title("Flasks")]
    public int totalFlaskAmount;
    public int normalFlaskMaxAmount;
    public int ashenFlaskMaxAmount;
    public int normalFlaskCurrentAmount;
    public int ashenFlaskCurrentAmount;
    public int consumableIndex;
    public int flaskLevel;
}
