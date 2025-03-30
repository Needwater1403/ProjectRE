using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class MessageContent
{ 
    [Title("Options")]
    public bool isConsoleInputHas2IconCombine;
    public bool isPCInputHas2IconCombine;
    [Title("Flags")]
    public bool has2IconCombine;
    [Space]
    public string prefix;
    public string middle;
    [TextArea] public string suffix;
}
