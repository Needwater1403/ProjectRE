using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Item : ScriptableObject
{
    [TabGroup("Item","Base")]
    [Title("Basic Info")]
    [SerializeField] public int ID;
    [TabGroup("Item", "Base")] 
    public int amount;
    [TabGroup("Item", "Base")] 
    public int basePriceInShop;
    [TabGroup("Item","Base")]
    public string name;
    [TabGroup("Item","Base")]
    public string baseName;
    [TabGroup("Item","Base")]
    public Sprite icon;
    [TabGroup("Item","Base")]
    [TextArea] public string description;
    [TabGroup("Item","Base")]
    public ItemType itemType;

}
