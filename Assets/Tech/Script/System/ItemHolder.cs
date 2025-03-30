using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ItemHolder
{
    public Item item;
    public int amount;
    [EnumToggleButtons] public ItemType itemType;
}
