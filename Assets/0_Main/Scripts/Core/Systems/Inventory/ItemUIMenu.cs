using System;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemUIMenu : MonoBehaviour
    {
        [SerializeField] private Button _selectBtn;
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _selectingIcon, _unSelectIcon;
        [SerializeField] private GameObject _selectingPanel;
        private Action _onSelect;

        private void Awake()
        {
            _selectBtn.onClick.AddListener(() => { Select(); });
        }

        public void Init(Action onSelect)
        {
            _onSelect = onSelect;
        }

        public virtual void Select()
        {
            _onSelect?.Invoke();
        }

        public void SelectStatus()
        {
            _selectingPanel.gameObject.SetActive(true);
            _icon.sprite = _selectingIcon;
            _selectBtn.interactable = false;
        }

        public void UnSelectStatus()
        {
            _selectBtn.interactable = true;
            _icon.sprite = _unSelectIcon;
            _selectingPanel.gameObject.SetActive(false);
        }
    }
}
