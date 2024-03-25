using UnityEngine;
using UnityEngine.UI;

public class Bar2 : MonoBehaviour
{
    [SerializeField] private Image _bg, _healthFill, _shieldFill;
    private Vector2 _originalSize;

    private void OnEnable()
    {
        _originalSize = _bg.rectTransform.sizeDelta;
    }

    public void Set(int maxHp, int currentHp, int shield)
    {
        float hpRate = (float)currentHp / (maxHp + shield);
        float shieldRate = (float)shield / (maxHp + shield);
        _healthFill.rectTransform.sizeDelta = new Vector2(_originalSize.x * hpRate, _originalSize.y);
        _shieldFill.rectTransform.sizeDelta = new Vector2(_originalSize.x * shieldRate, _originalSize.y);
    }
}
