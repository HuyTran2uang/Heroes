using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Unit
{
    [SerializeField] private int _id;
    [SerializeField] private int _level;

    public Unit(int id, int level)
    {
        _id = id;
        _level = level;
    }

    public int Id => _id;
    public int Level => _level;
}
