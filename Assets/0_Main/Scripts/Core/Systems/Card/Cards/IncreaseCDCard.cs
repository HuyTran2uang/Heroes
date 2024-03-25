using UnityEngine;

[CreateAssetMenu(fileName = "Increase CD Card", menuName = "Card/Increase CD Card")]
public class IncreaseCDCard : Card
{
    [SerializeField] private int _critDamage;

    public int CritDamage { get { return _critDamage; } protected set { _critDamage = value; } }

    public override string GetDescription() => $"+{CritDamage}% Crit Damage";

    public override void Use(PlayerController player)
    {
        player.CardStats.CritDamage += CritDamage;
    }
}
