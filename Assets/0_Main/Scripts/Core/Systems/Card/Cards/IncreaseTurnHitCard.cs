using UnityEngine;

[CreateAssetMenu(fileName = "Increase Turn Hit Card", menuName = "Card/Increase Turn Hit Card")]
public class IncreaseTurnHitCard : Card
{
    [SerializeField] private int _turn;

    public int Turn { get { return _turn; } protected set { _turn = value; } }

    public override string GetDescription() => $"Add Hit {_turn} Times";

    public override void Use(PlayerController player)
    {
        player.CardStats.HitEachTurn += Turn;
    }
}
