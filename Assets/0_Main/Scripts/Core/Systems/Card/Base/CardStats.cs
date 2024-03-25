using UnityEngine;

public class CardStats
{
    private PlayerController _player;
    [SerializeField] private int _healthPoint;
    [SerializeField] private int _critRate;
    [SerializeField] private int _critDamage;
    [SerializeField] private int _shield;
    [SerializeField] private int _hitEachTurn;
    [SerializeField] private int _meleeDam;
    [SerializeField] private int _rangeDam;
    [SerializeField] private int _lifesteal;
    [SerializeField] private int _block;
    [SerializeField] private int _ball;

    public int HealthPoint { get => _healthPoint; set => _healthPoint = value; }
    public int CritRate { get => _critRate; set => _critRate = value; }
    public int CritDamage { get => _critDamage; set => _critDamage = value; }
    public int Shield
    {
        get => _player.Shield;
        set
        {
            _player.Shield = value;
        }
    }
    public int HitEachTurn { get => _hitEachTurn; set => _hitEachTurn = value; }
    public int MeleeDam { get => _meleeDam; set => _meleeDam = value; }
    public int RangeDam { get => _rangeDam; set => _rangeDam = value; }
    public int Lifesteal { get => _lifesteal; set => _lifesteal = value; }
    public int Block { get => _block; set => _block = value; }
    public int Ball { get => _ball; set => _ball = value; }

    public CardStats(PlayerController player)
    {
        _player = player;
    }
}
