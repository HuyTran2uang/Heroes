using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    [SerializeField] private int _x;
    [SerializeField] private int _y;
    [SerializeField] private Vector3 _position;
    [SerializeField] private UnitController _unit;

    public int X => _x;
    public int Y => _y;
    public Vector3 Position => _position;

    public Node(int x, int y, Vector3 position)
    {
        _x = x;
        _y = y;
        _position = position;
    }

    public void Lock(UnitController unit)
    {
        _unit = unit;
    }

    public void Unlock()
    {
        _unit = null;
    }

    public bool IsLocked => _unit != null;
}
