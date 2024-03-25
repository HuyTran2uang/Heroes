using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _txt;
    private int _dotCount;
    private Action _callback;

    private void OnEnable()
    {
        _txt.text = "Loading";
        StartCoroutine(Loading(.5f));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Loading(float duration)
    {
        yield return new WaitForSeconds(duration);
        _callback?.Invoke();
        if (_dotCount == 0)
        {
            _callback = AddDot;
        }
        if (_dotCount == 3)
        {
            _callback = RemoveDot;
        }
        StartCoroutine(Loading(duration));
    }

    private void AddDot()
    {
        _txt.text += ".";
        _dotCount++;
    }

    private void RemoveDot()
    {
        _txt.text = _txt.text.Remove(_txt.text.Length - 1);
        _dotCount--;
    }
}
