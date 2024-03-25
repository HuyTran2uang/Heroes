using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stage
{
    [SerializeField] private Unit[] _units;

    public Stage(Unit[] units)
    {
        _units = units;
    }

    public Unit[] Units => _units;
}
