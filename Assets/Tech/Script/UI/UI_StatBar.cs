using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    [SerializeField] protected Image _image;
    [SerializeField] protected Slider _slider;
    public virtual void SetStat(float newValue)
    {
        _image.fillAmount = newValue;
    }

    public virtual void SetMaxStat(float maxValue)
    {
        _image.fillAmount = 1;
    }
}
