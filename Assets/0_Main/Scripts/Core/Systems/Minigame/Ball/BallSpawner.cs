using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Ball
{
    public class BallSpawner : MonoBehaviourSingleton<BallSpawner>
    {
        [SerializeField] protected BallController _prefab;
        [SerializeField] protected Transform _container;
        protected Queue<BallController> _pool = new Queue<BallController>();
        protected List<BallController> _using = new List<BallController>();
        private int _id;

        public void ResetSetId()
        {
            _id = 0;
        }

        public BallController Spawn()
        {
            BallController item;
            _id++;
            if (_pool.Count > 0)
            {
                item = _pool.Dequeue();
            }
            else
            {
                item = Instantiate(_prefab, _container);
            }
            _using.Add(item);
            item.Id = _id;
            return item;
        }

        public void AddToPool(BallController item)
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
}
