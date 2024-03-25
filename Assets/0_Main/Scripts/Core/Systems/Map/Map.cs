using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Map
{
    [SerializeField] private int _id;
    [SerializeField] private Stage[] _stages;

    public Map(int id, Stage[] stages)
    {
        _id = id;
        _stages = stages;
    }

    public int Id => _id;
    public Stage[] Stages => _stages;
}
