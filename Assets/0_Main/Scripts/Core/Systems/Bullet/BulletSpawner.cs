using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletSpawner : MonoBehaviourSingleton<BulletSpawner>
{
    private Dictionary<BulletType, BulletController> _prefabs = new Dictionary<BulletType, BulletController>();
    private Dictionary<BulletType, Queue<BulletController>> _pool = new Dictionary<BulletType, Queue<BulletController>>();
    private Dictionary<BulletType, List<BulletController>> _using = new Dictionary<BulletType, List<BulletController>>();

    private void Awake()
    {
        Resources.LoadAll<BulletController>("Prefabs/Bullets").ToList().ForEach(i => _prefabs.Add(i.Type, i));
    }

    public BulletController Spawn(BulletType type)
    {
        BulletController item = null;
        if (!_pool.ContainsKey(type))
        {
            _pool.Add(type, new Queue<BulletController>());
        }

        if (_pool[type].Count > 0)
        {
            item = _pool[type].Dequeue();
        }
        else
        {
            item = Instantiate(_prefabs[type], transform);
        }

        if(!_using.ContainsKey(type))
        {
            _using.Add(type, new List<BulletController>());
        }

        _using[type].Add(item);

        return item;
    }

    public void AddToPool(BulletController bullet)
    {
        if (!_pool.ContainsKey(bullet.Type))
        {
            _pool.Add(bullet.Type, new Queue<BulletController>());
        }

        _using[bullet.Type].Remove(bullet);
        _pool[bullet.Type].Enqueue(bullet);
    }

    public void Clear()
    {
        foreach (var listBullet in _using.Values)
        {
            for (var i = listBullet.Count - 1; i == 0; i--)
            {
                listBullet[i].gameObject.SetActive(false);
            }
        }
    }
}