using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPage : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void SetLoadingProgressBar(int current, int max)
    {
        _slider.maxValue = max;
        _slider.value = current;
    }
}
