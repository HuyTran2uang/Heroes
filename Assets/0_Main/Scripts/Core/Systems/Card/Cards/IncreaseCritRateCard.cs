using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Increase Crit Rate Card", menuName = "Card/Increase Crit Rate Card")]
public class IncreaseCritRateCard : Card
{
    [SerializeField] private int _critRate;

    public int CritRate { get { return _critRate; } protected set { _critRate = value; } }

    public override string GetDescription() => $"+{_critRate}% Crit Change";

    public override void Use(PlayerController player)
    {
        player.CardStats.CritRate += CritRate;
    }
}
