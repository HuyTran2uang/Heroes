using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixSlider : Slider
{
    [SerializeField] private Image _fillValue;

    protected override void Set(float input, bool sendCallback = true)
    {
        base.Set(input, sendCallback);
        if (_fillValue != null)
            _fillValue.fillAmount = m_Value;
    }
}
