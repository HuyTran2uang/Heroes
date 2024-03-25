using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] private DamageText _prefab;
    [SerializeField] private Transform _container;
    Queue<DamageText> _pool = new Queue<DamageText>();
    List<DamageText> _using = new List<DamageText>();

    public DamageText Spawn()
    {
        DamageText item;
        if (_pool.Count > 0)
            item = _pool.Dequeue();
        else
        {
            item = Instantiate(_prefab, _container);
            item.OnDisabled = AddToPool;
        }

        _using.Add(item);

        return item;
    }

    public void AddToPool(DamageText item)
    {
        _using.Remove(item);
        _pool.Enqueue(item);
    }

    public void Clear()
    {
        for (int i = _using.Count - 1; i == 0; i--)
        {
            _using[i].gameObject.SetActive(false);
        }
    }
}
