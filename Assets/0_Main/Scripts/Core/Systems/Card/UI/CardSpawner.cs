using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviourSingleton<CardSpawner>
{
    [SerializeField] private CardItemUI _cardPrefab;
    [SerializeField] private Transform _container;
    private Queue<CardItemUI> _pool = new Queue<CardItemUI>();
    private List<CardItemUI> _using = new List<CardItemUI>();

    public CardItemUI Spawn()
    {
        CardItemUI item;
        if (_pool.Count > 0)
            item = _pool.Dequeue();
        else
            item = Instantiate(_cardPrefab, _container);

        _using.Add(item);

        return item;
    }

    public void AddToPool(CardItemUI item)
    {
        _using.Remove(item);
        _pool.Enqueue(item);
    }
}
