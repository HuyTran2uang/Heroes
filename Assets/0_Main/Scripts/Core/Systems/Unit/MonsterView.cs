using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterView : MonoBehaviour
{
    [SerializeField] private Bar _healthBar;
    [SerializeField] private DamageTextSpawner _damageTextSpawner;
    private CanvasView _canvasView;
    [SerializeField] private GameObject _meleeWeapon;
    [SerializeField] private GameObject _rangeWeapon;
    private Animator _animator;

    public Bar HealthBar => _healthBar;
    public DamageTextSpawner DamageTextSpawner
    {
        get
        {
            if(_damageTextSpawner == null)
            {
                _damageTextSpawner = GetComponentInChildren<DamageTextSpawner>(true);
            }
            return _damageTextSpawner;
        }
    }
    public CanvasView CanvasView
    {
        get
        {
            if (_canvasView == null)
            {
                _canvasView = GetComponentInChildren<CanvasView>(true);
            }
            return _canvasView;
        }
    }
    public Animator Animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>(true);
            }
            return _animator;
        }
    }

    public void Clear()
    {
        DamageTextSpawner.Clear();
    }
    public void ChangeWeapon(bool isMelee)
    {
        _meleeWeapon?.SetActive(isMelee);
        _rangeWeapon?.SetActive(!isMelee);
    }
}
