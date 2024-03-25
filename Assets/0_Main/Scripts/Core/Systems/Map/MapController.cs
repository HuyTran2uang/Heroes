using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private Map _map;
    [SerializeField] private Transform _gate1, _gate2;

    public int Id => _id;
    public Map Map => _map;
    public Transform Gate1 => _gate1;
    public Transform Gate2 => _gate2;

    private void OnEnable()
    {
        Init(_id);
    }

    private void OnDisable()
    {
        MapSpawner.Instance.AddToPool(this);
    }

    public void Init(int mapId)
    {
        _map = MapManager.Instance.GetMap(mapId);
    }

    public void Init()
    {
        _map = MapManager.Instance.GetMap(_id);
        gameObject.SetActive(true);
    }
}
