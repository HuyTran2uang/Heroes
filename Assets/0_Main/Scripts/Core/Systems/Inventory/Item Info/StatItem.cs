using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class StatItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name, _valueText;

        public void Init(Sprite icon, string name, string value)
        {
            _icon.gameObject.SetActive(icon != null);
            _icon.sprite = icon;
            _name.text = name;
            _valueText.text = value;
            gameObject.SetActive(true);
        }
    }
}
