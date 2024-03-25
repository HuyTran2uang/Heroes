using UnityEngine;

[CreateAssetMenu]
public class MonsterBaseStats : ScriptableObject
{
    [SerializeField] private int _hp;
    [SerializeField] private int _atk;
    [SerializeField] private int _critRate;
    [SerializeField] private int _critDamage;
    [SerializeField] private int _ball;

    public int Hp { get => _hp; protected set => _hp = value; }
    public int Atk { get => _atk; protected set => _atk = value; }
    public int CritRate { get => _critRate; protected set => _critRate = value; }
    public int CritDamage { get => _critDamage; protected set => _critDamage = value; }
    public int Ball { get => _ball; protected set => _ball = value; }
}
