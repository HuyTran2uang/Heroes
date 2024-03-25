using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviourSingleton<InventoryManager>
    {
        [SerializeField] private List<Item> _items = new List<Item>();
        private System.Type _type;
        private int _selectingItemIndex;

        private void Awake()
        {
            var itemSOs = Resources.LoadAll<ItemSO>("SO/Items").ToList();

            string data = PlayerPrefs.HasKey(Config.Key_ItemOwner) ? PlayerPrefs.GetString(Config.Key_ItemOwner) : Config.Default_ItemOwner;
            List<string> itemOwnerNames = data.Split(',').ToList();

            itemSOs.ForEach(i =>
            {
                Item item = new Item(i);
                bool isOwener = itemOwnerNames.Any(j => j == i.Name);
                item.IsOwner = isOwener;
                _items.Add(item);
            });


        }

        private void Start()
        {
            _type = typeof(MeleeWeapon);

            //equipment
            var meleeWeapon = !PlayerPrefs.HasKey(Config.Key_MeleeWeapon) ? Config.Default_MeleeWeapon : PlayerPrefs.GetString(Config.Key_MeleeWeapon);
            var rangeWeapon = !PlayerPrefs.HasKey(Config.Key_RangeWeapon) ? Config.Default_RangeWeapon : PlayerPrefs.GetString(Config.Key_RangeWeapon);
            var armor = !PlayerPrefs.HasKey(Config.Key_Armor) ? Config.Default_ArmorWeapon : PlayerPrefs.GetString(Config.Key_Armor);
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].ItemSO.Name == meleeWeapon)
                {
                    _items[i].Use();
                }
                else if (_items[i].ItemSO.Name == rangeWeapon)
                {
                    _items[i].Use();
                }
                else if (_items[i].ItemSO.Name == armor)
                {
                    _items[i].Use();
                }
            }

            ShowListMeleeWeapon();
        }

        public void ShowListMeleeWeapon()
        {
            _type = typeof(MeleeWeapon);
            var items = _items.Where(i => i.ItemSO.GetType() == typeof(MeleeWeapon)).ToList();
            int index = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].IsEquipping)
                {
                    index = i;
                    break;
                }
            }
            GameController.Instance.View.MainPage.InventoryPanel.ShowListItem(items);
            GameController.Instance.View.MainPage.InventoryPanel.MeleeMenu.SelectStatus();
            Select(index);
        }

        public void ShowListRangeWeapon()
        {
            _type = typeof(RangeWeapon);
            var items = _items.Where(i => i.ItemSO.GetType() == typeof(RangeWeapon)).ToList();
            int index = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].IsEquipping)
                {
                    index = i;
                    break;
                }
            }
            GameController.Instance.View.MainPage.InventoryPanel.ShowListItem(items);
            GameController.Instance.View.MainPage.InventoryPanel.RangeMenu.SelectStatus();
            Select(index);
        }

        public void ShowListArmor()
        {
            _type = typeof(Armor);
            var items = _items.Where(i => i.ItemSO.GetType() == typeof(Armor)).ToList();
            int index = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].IsEquipping)
                {
                    index = i;
                    break;
                }
            }
            GameController.Instance.View.MainPage.InventoryPanel.ShowListItem(items);
            GameController.Instance.View.MainPage.InventoryPanel.ArmorMenu.SelectStatus();
            Select(index);
        }

        public void Select(int index)
        {
            GameController.Instance.View.MainPage.InventoryPanel.ItemInfoPanel.Clear();
            _selectingItemIndex = index;
            var items = _items.Where(i => i.ItemSO.GetType() == _type).ToList();
            Item item = items[index];
            item.ItemSO.Show();
            GameController.Instance.View.MainPage.InventoryPanel.ItemInfoPanel.Init(
                item.ItemSO.Image,
                item.ItemSO.Name
            );

            GameController.Instance.View.MainPage.InventoryPanel.SelectItem(index);

            if (item.IsOwner)
            {
                if (item.IsEquipping)
                {
                    GameController.Instance.View.MainPage.InventoryPanel.SetFlexibleBtn(null, $"Equipped", null, false);
                }
                else
                {
                    GameController.Instance.View.MainPage.InventoryPanel.SetFlexibleBtn(null, $"Equip", Use, true);
                }
            }
            else
            {
                bool canBuy = false;
                switch (item.ItemSO.PriceType)
                {
                    case PriceType.Gold:
                        Sprite goldIcon = Resources.Load<Sprite>("Sprites/Icon/Icon_Gold");
                        canBuy = CurrencyManager.Instance.Gold >= item.ItemSO.Price;
                        GameController.Instance.View.MainPage.InventoryPanel.SetFlexibleBtn(goldIcon, $"{item.ItemSO.Price}", Buy, canBuy);
                        break;
                    case PriceType.Diamond:
                        canBuy = CurrencyManager.Instance.Diamond >= item.ItemSO.Price;
                        GameController.Instance.View.MainPage.InventoryPanel.SetFlexibleBtn(null, $"{item.ItemSO.Price}", Buy, canBuy);
                        break;
                    case PriceType.Ads:
                        break;
                }
            }
        }


        public void Use()
        {
            var items = _items.Where(i => i.ItemSO.GetType() == _type).ToList();
            Item item = items[_selectingItemIndex];
            item.Use();
            items.Where(i => i != item).ToList().ForEach(i => i.IsEquipping = false);
            GameController.Instance.View.MainPage.InventoryPanel.SetFlexibleBtn(null, $"Equipped", null, false);
        }

        public void Buy()
        {
            var items = _items.Where(i => i.ItemSO.GetType() == _type).ToList();
            Item item = items[_selectingItemIndex];
            if (item.IsOwner) return;
            switch (item.ItemSO.PriceType)
            {
                case PriceType.Gold:
                    if (CurrencyManager.Instance.UseGold(item.ItemSO.Price))
                    {
                        item.IsOwner = true;
                        GameController.Instance.View.MainPage.InventoryPanel.Unlock(_selectingItemIndex);
                        GameController.Instance.View.MainPage.InventoryPanel.SetFlexibleBtn(null, "Equip", Use, true);
                        string data = PlayerPrefs.HasKey(Config.Key_ItemOwner) ? PlayerPrefs.GetString(Config.Key_ItemOwner) : Config.Default_ItemOwner;
                        PlayerPrefs.SetString(Config.Key_ItemOwner, data + $",{item.ItemSO.Name}");
                    }
                    break;
                case PriceType.Diamond:
                    if (CurrencyManager.Instance.UseDiamond(item.ItemSO.Price))
                    {
                        item.IsOwner = true;
                        GameController.Instance.View.MainPage.InventoryPanel.Unlock(_selectingItemIndex);
                        GameController.Instance.View.MainPage.InventoryPanel.SetFlexibleBtn(null, "Equip", Use, true);
                        string data = PlayerPrefs.HasKey(Config.Key_ItemOwner) ? PlayerPrefs.GetString(Config.Key_ItemOwner) : Config.Default_ItemOwner;
                        PlayerPrefs.SetString(Config.Key_ItemOwner, data + $",{item.ItemSO.Name}");
                    }
                    break;
                case PriceType.Ads:
                    break;
            }
        }
    }
}
