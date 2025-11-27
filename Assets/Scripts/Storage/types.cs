using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class DefaulComponents
{
    public Transform FieldContainer => _fieldContainer;
    [SerializeField] protected Transform _fieldContainer;

    public Transform SymbolContainer => _symbolContainer;
    [SerializeField] protected Transform _symbolContainer;
}

public class WinCombination
{
    public string ID => _id;
    private string _id;
    public List<CellPosition> Positions => _positions;
    private List<CellPosition> _positions = new();
    public WinType WinType => _winType;
    private WinType _winType;

    public WinCombination(
        string id,
        List<CellPosition> positions,
        WinType winType
    )
    {
        _id = id;
        _positions = positions;
        _winType = winType;
    }
}

public class CellPosition
{
    public int X => _x;
    private int _x;
    public int Y => _y;
    private int _y;

    public CellPosition(
        int x,
        int y
    )
    {
        _x = x;
        _y = y;
    }

    public void UpdatePos(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public CellPosition Clone()
    {
        return new(_x, _y);
    }
}

public class Constants
{
    public readonly float MoveTime = 0.5f;
    public readonly float ShiftTime = 0.2f;
    public readonly float ShiftMove = 0.2f;
    public readonly float DropStartPosY = 5f;
}

public enum Direction
{
    horizontal = 0,
    horizontalLeft = 2,
    horizontalRight = 3,
    vertical = 4,
    vertivalTop = 5,
    verticalBottom = 6
}