using UnityEngine;

[CreateAssetMenu(fileName = "Increase Shield Card", menuName = "Card/Increase Shield Card")]
public class IncreaseShieldCard : Card
{
    [SerializeField] private int _shield;

    public int Shield { get { return _shield; } protected set { _shield = value; } }

    public override string GetDescription() => $"+{Shield} Shield";

    public override void Use(PlayerController player)
    {
        player.CardStats.Shield += Shield;
    }
}
