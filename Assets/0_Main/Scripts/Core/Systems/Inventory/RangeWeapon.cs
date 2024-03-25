using Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "Range Weapon", menuName = "Item/Equipment/Weapon/Range Weapon")]
public class RangeWeapon : Weapon
{
    [SerializeField] private int _rangeAtk;

    public int RangeAtk => _rangeAtk;

    public override void Use()
    {
        EquipmentManager.Instance.Equip(this);
    }

    public override void Show()
    {
        ItemInfoPanel info = GameController.Instance.View.MainPage.InventoryPanel.ItemInfoPanel;
        info.AddStat("Dam", $"{RangeAtk}");
    }
}
