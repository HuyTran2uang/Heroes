using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemInfoPanel : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private StatItem _statItemPrefab;
        [SerializeField] private Transform _statsContainer;
        List<StatItem> _pool = new List<StatItem>();
        int _currentStatIndex;

        public void Init(Sprite icon, string name)
        {
            _icon.sprite = icon;
            _name.text = name;
            gameObject.SetActive(true);
        }

        public void AddStat(string name, string value)
        {
            Sprite icon = Resources.Load<Sprite>($"Sprites/Icon/icon_{name}");

            var item = Spawn();
            item.Init(icon, name, value);
            _currentStatIndex++;
        }

        public void Clear()
        {
            _pool.ForEach(i => i.gameObject.SetActive(false));
            _currentStatIndex = 0;
        }

        public StatItem Spawn()
        {
            StatItem item;

            if(_currentStatIndex == _pool.Count)
            {
                item = Instantiate(_statItemPrefab, _statsContainer);
                _pool.Add(item);
            }

            item = _pool[_currentStatIndex];

            return item;
        }
    }
}
