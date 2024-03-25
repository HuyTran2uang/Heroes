using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private BulletType _type;
    private Vector3 _direction;
    private Action _onCompleted;
    private BulletSensor[] _sensors;
    private UnitController _shooter;
    private UnitController _target;

    public BulletType Type => _type;
    public Action OnCompleted => _onCompleted;
    public UnitController Target => _target;

    private void Awake()
    {
        _sensors = GetComponentsInChildren<BulletSensor>(true);
    }

    private void FixedUpdate()
    {
        transform.position += _direction * 10 * Time.deltaTime;
    }

    private void OnEnable()
    {
        StartCoroutine(AutoDisable(10));
        _sensors.ToList().ForEach(i => i.Init(this));
    }

    private void OnDisable()
    {
        BulletSpawner.Instance.AddToPool(this);
    }

    public void Init(UnitController shooter, UnitController target, Action onCompleted)
    {
        _shooter = shooter;
        _target = target;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.transform.position;
        targetPos.y = currentPos.y;

        _direction = Vector3.Normalize(targetPos - currentPos);
        _onCompleted = () =>
        {
            int damage = shooter.RangeDam;
            bool isCrited = UnityEngine.Random.Range(0f, 100f) < shooter.CritRate;
            damage = isCrited ? damage + (int)(damage * shooter.CritDamage / 100f) : damage;
            _target.TakeDamage(damage, isCrited, shooter, onCompleted);
            gameObject.SetActive(false);
        };
        gameObject.SetActive(true);
    }

    private IEnumerator AutoDisable(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}

public enum BulletType
{
    Normal,
    Arrow
}
