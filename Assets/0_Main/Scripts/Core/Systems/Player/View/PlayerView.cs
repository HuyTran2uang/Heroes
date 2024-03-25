using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Bar2 _healthBar;
    private DamageTextSpawner _damageTextSpawner;
    private CanvasView _canvasView;
    private Animator _animator;
    [SerializeField] private GameObject _meleeWeapon;
    [SerializeField] private GameObject _rangeWeapon;
    [SerializeField] private GameObject _model;

    public Bar2 HealthBar => _healthBar;
    public DamageTextSpawner DamageTextSpawner
    {
        get
        {
            if (_damageTextSpawner == null)
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
    public GameObject Model => _model;

    public void Initialize()
    {

    }

    public void ChangeWeapon(bool isMelee)
    {
        _meleeWeapon?.SetActive(isMelee);
        _rangeWeapon?.SetActive(!isMelee);
    }
}
