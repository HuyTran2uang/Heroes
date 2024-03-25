using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Item/Equipment/Armor")]
public class Armor : Equipment
{
    [SerializeField] private int _hp;
    
    public int HP => _hp;

    public override void Show()
    {
        ItemInfoPanel info = GameController.Instance.View.MainPage.InventoryPanel.ItemInfoPanel;
        if (HP != 0)
        {
            info.AddStat("Health", $"{HP}");
        }
    }

    public override void Use()
    {
        EquipmentManager.Instance.Equip(this);
    }
}
