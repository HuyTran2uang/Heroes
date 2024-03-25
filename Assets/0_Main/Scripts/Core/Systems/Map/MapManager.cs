using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviourSingleton<MapManager>
{
    private int _currentMap;
    private int _currentStage;
    private int _mapPassed;
    private Dictionary<int, Map> _mapDictionary = new Dictionary<int, Map>();
    [SerializeField] private List<Sprite> _mapSprites = new List<Sprite>();

    public int CurrentMap { get { return _currentMap; } private set { _currentMap = value; } }
    public int CurrentStage { get { return _currentStage; } set { _currentStage = value; } }
    public int MapPassed { get { return _mapPassed; } private set { _mapPassed = value; } }

    private void Awake()
    {
        var maps = Resources.LoadAll<TextAsset>("Configs/Maps").ToList().Select(i => JsonUtility.FromJson<Map>(i.text)).ToList();
        maps.ForEach(i => _mapDictionary.Add(i.Id, i));

        _mapSprites = Resources.LoadAll<Sprite>("Sprites/Maps").ToList();
    }

    public void Initialize()
    {
        UpdateView(CurrentMap, MapPassed, _mapDictionary.Count);
    }

    public Map GetMap(int id)
    {
        return _mapDictionary[id];
    }

    public int Count()
    {
        return _mapDictionary.Count;
    }

    private void UpdateView(int current, int passed, int count)
    {
        Sprite imageMap = current == count ? null : _mapSprites[current];
        GameController.Instance.View.MainPage.SetMap(imageMap);
        GameController.Instance.View.MainPage.SetMapText(current);
        GameController.Instance.View.MainPage.DisplaySelectMapBtns(current, passed, count);
    }

    public void CompletedCurrentMap()
    {
        CurrentMap++;
        if (CurrentMap > MapPassed) MapPassed++;
        UpdateView(CurrentMap, MapPassed, _mapDictionary.Count);
    }

    public void SelectNextMap()
    {
        if (CurrentMap == MapPassed) return;
        CurrentMap++;
        UpdateView(CurrentMap, MapPassed, _mapDictionary.Count);
    }

    public void SelectPrevMap()
    {
        if (CurrentMap == 0) return;
        CurrentMap--;
        UpdateView(CurrentMap, MapPassed, _mapDictionary.Count);
    }

#if UNITY_EDITOR
    [SerializeField] private Map[] _maps;

    public void CacheMap()
    {
        _maps.ToList().ForEach(i => CacheMap(i));
    }

    private void CacheMap(Map map)
    {
        File.WriteAllText($"{Application.dataPath}/_Main/Resources/Configs/Maps/{map.Id}.txt", JsonUtility.ToJson(map));
    }
#endif
}
