using UnityEngine;

[CreateAssetMenu(fileName = "Increase Hp Card", menuName = "Card/Increase Hp Card")]
public class IncreaseHpCard : Card
{
    [SerializeField] private int _hp;

    public int Hp { get { return _hp; } protected set { _hp = value; } }

    public override string GetDescription() => $"+{Hp} Health";

    public override void Use(PlayerController player)
    {
        player.CardStats.HealthPoint += Hp;
        player.HealthRegen(Hp);
    }
}
