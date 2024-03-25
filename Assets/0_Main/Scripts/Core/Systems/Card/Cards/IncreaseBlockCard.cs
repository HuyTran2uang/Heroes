using UnityEngine;

[CreateAssetMenu(fileName = "Increase Block Card", menuName = "Card/Increase Block Card")]
public class IncreaseBlockCard : Card
{
    [SerializeField] private int _block;

    public int Block { get { return _block; } protected set { _block = value; } }

    public override string GetDescription() => $"Block {Block} Damage";

    public override void Use(PlayerController player)
    {
        player.CardStats.Block += Block;
    }
}
