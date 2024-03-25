using UnityEngine;

[CreateAssetMenu(fileName = "Increase Damage Card", menuName = "Card/Increase Damage Card")]
public class IncreaseDamageCard : Card
{
    [SerializeField] private int _attackPoint;

    public int AttackPoint { get { return _attackPoint; } protected set { _attackPoint = value; } }

    public override string GetDescription() => $"+{AttackPoint} Damage";

    public override void Use(PlayerController player)
    {
        player.CardStats.MeleeDam += AttackPoint;
        player.CardStats.RangeDam += AttackPoint;
    }
}
