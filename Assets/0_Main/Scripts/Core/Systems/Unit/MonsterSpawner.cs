using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviourSingleton<MonsterSpawner>
{
    private Dictionary<int, MonsterController> _prefabs = new Dictionary<int, MonsterController>();
    //private Dictionary<int, Queue<MonsterController>> _pool = new Dictionary<int, Queue<MonsterController>>();
    //private Dictionary<int, List<MonsterController>> _using = new Dictionary<int, List<MonsterController>>();
    private List<MonsterController> _monsters = new List<MonsterController>();

    private void Awake()
    {
        Resources.LoadAll<MonsterController>("Prefabs/Monsters").ToList().ForEach(i => _prefabs.Add(i.Id, i));
    }

    public MonsterController Spawn(Unit unit)
    {
        MonsterController monster;

        monster = Instantiate(_prefabs[unit.Id], transform);
        _monsters.Add(monster);
        //if (!_pool.ContainsKey(unit.Id))
        //    _pool.Add(unit.Id, new Queue<MonsterController>());

        //if (_pool[unit.Id].Count > 0)
        //    monster = _pool[unit.Id].Dequeue();
        //else
        //    monster = Instantiate(_prefabs[unit.Id], transform);

        //if (!_using.ContainsKey(unit.Id))
        //    _using.Add(unit.Id, new List<MonsterController>());

        //_using[unit.Id].Add(monster);

        return monster;
    }

    public void AddToPool(MonsterController monster)
    {
        //if (!_using.ContainsKey(monster.Id))
        //    _using.Add(monster.Id, new List<MonsterController>());

        //_using[monster.Id].Remove(monster);

        //_pool[monster.Id].Enqueue(monster);
    }

    public void Clear()
    {
        _monsters.ForEach(i => Destroy(i.gameObject));
        _monsters.Clear();
        //foreach (var listMonster in _using.Values)
        //{
        //    for (int i = listMonster.Count - 1; i == 0; i++)
        //    {
        //        listMonster[i].gameObject.SetActive(false);
        //    }
        //}
    }
}
