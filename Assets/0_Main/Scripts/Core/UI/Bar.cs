using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Transform _bar;
    [SerializeField] private Image _fill;

    public void Set(int max, int current)
    {
        float rate = (float)current / max;
        _fill.transform.localScale = new Vector3(rate, 1, 1);
    }
}
