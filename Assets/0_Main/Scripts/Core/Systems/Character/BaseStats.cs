using UnityEngine;

[System.Serializable]
public class BaseStats
{
    [SerializeField] private int _healthPoint;
    [SerializeField] private int _critRate;
    [SerializeField] private int _critDamage;
    [SerializeField] private int _hitEachTurn;
    [SerializeField] private int _meleeDam;
    [SerializeField] private int _rangeDam;

    public int HealthPoint { get => _healthPoint; private set => _healthPoint = value; }
    public int CritRate { get => _critRate; private set => _critRate = value; }
    public int CritDamage { get => _critDamage; private set => _critDamage = value; }
    public int HitEachTurn { get => _hitEachTurn; private set => _hitEachTurn = value; }
    public int MeleeDam { get => _meleeDam; private set => _meleeDam = value; }
    public int RangeDam { get => _rangeDam; private set => _rangeDam = value; }

    public BaseStats(int healthPoint, int critRate, int critDamage, int hitEachTurn, int meleeDam, int rangeDam)
    {
        _healthPoint = healthPoint;
        _critRate = critRate;
        _critDamage = critDamage;
        _hitEachTurn = hitEachTurn;
        _meleeDam = meleeDam;
        _rangeDam = rangeDam;
    }
}
