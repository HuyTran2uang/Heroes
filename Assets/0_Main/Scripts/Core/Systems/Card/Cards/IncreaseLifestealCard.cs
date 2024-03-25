using UnityEngine;

[CreateAssetMenu(fileName = "Increase Lifesteal Card", menuName = "Card/Increase Lifesteal Card")]
public class IncreaseLifestealCard : Card
{
    [SerializeField] private int _lifesteal;

    public int Lifesteal { get { return _lifesteal; } protected set { _lifesteal = value; } }

    public override string GetDescription() => $"+{Lifesteal}% Life Steal";

    public override void Use(PlayerController player)
    {
        player.CardStats.Lifesteal += Lifesteal;
    }
}
