using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapSpawner : MonoBehaviourSingleton<MapSpawner>
{
    Dictionary<int, MapController> _prefabs = new Dictionary<int, MapController>();
    Dictionary<int, MapController> _pool = new Dictionary<int, MapController>();
    Dictionary<int, MapController> _using = new Dictionary<int, MapController>();

    private void Awake()
    {
        Resources.LoadAll<MapController>("Prefabs/Maps").ToList().ForEach(i => _prefabs.Add(i.Id, i));
    }

    public MapController Spawn(int id)
    {
        MapController map;

        if (_pool.ContainsKey(id))
        {
            map = _pool[id];
            _pool.Remove(id);
        }
        else
            map = Instantiate(_prefabs[id], transform);

        _using.Add(id, map);

        return map;
    }

    public void AddToPool(MapController map)
    {
        _pool.Add(map.Id, map);
        _using.Remove(map.Id);
    }
}
