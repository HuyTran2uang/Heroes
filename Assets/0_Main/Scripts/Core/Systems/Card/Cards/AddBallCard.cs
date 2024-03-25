using UnityEngine;

[CreateAssetMenu(fileName = "Add Ball Card", menuName = "Card/Add Ball Card")]
public class AddBallCard : Card
{
    [SerializeField] private int _ball;

    public int Ball { get { return _ball; } protected set { _ball = value; } }

    public override string GetDescription() => $"Additional {Ball} Balls Per Kill";

    public override void Use(PlayerController player)
    {
        player.CardStats.Ball += Ball;
    }
}
