using Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Weapon", menuName = "Item/Equipment/Weapon/Melee Weapon")]
public class MeleeWeapon : Weapon
{
    [SerializeField] private int _meleeAtk;

    public int MeleeAtk => _meleeAtk;

    public override void Use()
    {
        EquipmentManager.Instance.Equip(this);
    }

    public override void Show()
    {
        ItemInfoPanel info = GameController.Instance.View.MainPage.InventoryPanel.ItemInfoPanel;
        info.AddStat("Dam", $"{MeleeAtk}");
    }
}
