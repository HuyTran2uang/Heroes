using UnityEngine;
using static UnityEditor.Progress;

namespace Inventory
{
    [System.Serializable]
    public class Item
    {
        [SerializeField] private ItemSO _itemSO;
        [SerializeField] private bool _isOwner;
        [SerializeField] private bool _isEquipping;

        public bool IsOwner
        {
            get => _isOwner;
            set
            {
                _isOwner = value;
            }
        }
        public bool IsEquipping
        {
            get => _isEquipping;
            set => _isEquipping = value;
        }

        public ItemSO ItemSO
        {
            get { return _itemSO; }
            private set { _itemSO = value; }
        }

        public Item(ItemSO itemSO)
        {
            ItemSO = itemSO;
        }

        public void Use()
        {
            _itemSO.Use();
            _isEquipping = true;
        }
    }
}
