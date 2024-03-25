using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField] private Button _backBtn, _flexibleBtn;
        [SerializeField] private Image _flexibleIcon;
        [SerializeField] private TMP_Text _flexibleText;
        [SerializeField] private ItemUIMenu _meleeMenu, _rangeMenu, _armorMenu;
        [SerializeField] private ItemInfoPanel _itemInfoPanel;
        [SerializeField] private ItemUI _itemUIPrefab;
        [SerializeField] private Transform _itemInventoryContainer;
        List<ItemUI> _pool = new List<ItemUI>();
        private PreviewController _previewController;

        public ItemInfoPanel ItemInfoPanel => _itemInfoPanel;
        public Action OnFlexibleEvent;
        public ItemUIMenu MeleeMenu => _meleeMenu;
        public ItemUIMenu RangeMenu => _rangeMenu;
        public ItemUIMenu ArmorMenu => _armorMenu;
        public PreviewController PreviewController
        {
            get
            {
                if(_previewController == null)
                {
                    _previewController = FindObjectOfType<PreviewController>(true);
                }
                return _previewController;
            }
        }

        private void Awake()
        {
            _backBtn.onClick.AddListener(() => { Hide(); });
            _meleeMenu?.Init(() => { ShowListMeleeWeapon(); });
            _rangeMenu?.Init(() => { ShowListRangeWeapon(); });
            _armorMenu?.Init(() => { ShowListArmorWeapon(); });
            _flexibleBtn.onClick.AddListener(() => { OnFlexibleEvent?.Invoke(); });
        }

        private void OnEnable()
        {
            PreviewController.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            PreviewController.gameObject.SetActive(false);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SelectItem(ItemUI itemUI)
        {
            int index = _pool.IndexOf(itemUI);
            InventoryManager.Instance.Select(index);
        }

        public void SelectItem(int index)
        {
            _pool[index].SelectStatus();
            for (int i = 0; i < _pool.Count; i++)
            {
                if (index != i)
                {
                    _pool[i].UnSelectStatus();
                }
            }
        }

        public void SetFlexibleBtn(Sprite icon, string content, Action callback, bool isInteracable)
        {
            _flexibleIcon.gameObject.SetActive(icon != null);
            _flexibleIcon.sprite = icon;
            _flexibleText.text = content;
            OnFlexibleEvent = callback;
            _flexibleBtn.interactable = isInteracable;
        }

        public void Clear()
        {
            _pool.ForEach(i => i.gameObject.SetActive(false));
            _itemInfoPanel.gameObject.SetActive(false);
            _pool.ForEach(i => i.UnSelectStatus());
            _meleeMenu.UnSelectStatus();
            _rangeMenu.UnSelectStatus();
            _armorMenu.UnSelectStatus();
        }

        public void ShowListItem(List<Item> items)
        {
            Clear();
            for (int i = 0; i < items.Count; i++)
            {
                var item = Spawn(i);
                item.OnSelect = SelectItem;
                item.gameObject.SetActive(true);
                item.Init(items[i].ItemSO.Image, !items[i].IsOwner);
            }
        }

        public void Unlock(int index)
        {
            _pool[index].UnLock();
        }

        private void ShowListMeleeWeapon()
        {
            InventoryManager.Instance.ShowListMeleeWeapon();
        }

        private void ShowListRangeWeapon()
        {
            InventoryManager.Instance.ShowListRangeWeapon();
        }

        private void ShowListArmorWeapon()
        {
            InventoryManager.Instance.ShowListArmor();
        }

        public ItemUI Spawn(int index)
        {
            ItemUI item;

            if (_pool.Count <= index)
            {
                item = Instantiate(_itemUIPrefab, _itemInventoryContainer);
                _pool.Add(item);
            }

            item = _pool[index];

            return item;
        }
    }
}
