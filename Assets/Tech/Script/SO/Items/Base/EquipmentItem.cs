using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EquipmentItem : Item
{
   [TabGroup("Item","Equipment")]
   public int personalID;
   [TabGroup("Item","Equipment")]
   public float weight;
   [TabGroup("Item","Equipment")]
   public int level;
   public virtual void SetLevel(int _level)
   {
      level = _level;
      if (level != 0) name = $"{name} + {level}";
   }
}
