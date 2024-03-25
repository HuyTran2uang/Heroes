using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text _txt;
    public Action<DamageText> OnDisabled;

    private void OnEnable()
    {
        _txt.rectTransform.DOAnchorPosY(70, 1f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDisable()
    {
        _txt.rectTransform.anchoredPosition = Vector3.zero;
        OnDisabled?.Invoke(this);
    }

    public void Show(int damage, StatusText status)
    {
        string icon = "";
        switch (status)
        {
            case StatusText.NormalDamage:
                _txt.color = Color.white;
                break;
            case StatusText.CritDamage:
                _txt.color = Color.red;
                icon = "<sprite index=0>";
                break;
            case StatusText.Health:
                _txt.color = Color.green;
                break;
            default:
                _txt.color = Color.white;
                break;
        }
        _txt.text = $"{icon}{damage}";
        gameObject.SetActive(true);
    }
}

public enum StatusText
{
    NormalDamage,
    CritDamage,
    Health
}
