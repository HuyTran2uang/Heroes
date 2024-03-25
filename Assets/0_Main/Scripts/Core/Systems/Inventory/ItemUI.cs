using System;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField] private Button _selectBtn;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selectPanel;
        public Action<ItemUI> OnSelect;
        [SerializeField] private GameObject _lock;

        private void Awake()
        {
            _selectBtn.onClick.AddListener(() => { Select(); });
        }

        public void Init(Sprite icon, bool isLock)
        {
            _icon.sprite = icon;
            _icon.gameObject.SetActive(icon != null);
            _lock.SetActive(isLock);
        }

        public void UnLock()
        {
            _lock.SetActive(false);
        }

        public void Select()
        {
            OnSelect?.Invoke(this);
        }

        public void SelectStatus()
        {
            _selectPanel.gameObject.SetActive(true);
            _selectBtn.interactable = false;
        }

        public void UnSelectStatus()
        {
            _selectPanel.gameObject.SetActive(false);
            _selectBtn.interactable = true;
        }
    }
}
