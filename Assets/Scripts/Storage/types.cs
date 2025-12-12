using System.Collections.Generic;
using UnityEngine;

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

    public void SetWinType(WinType type)
    {
        if (_winType != WinType.NotSelected) return;
        _winType = type;
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

public enum Direction
{
    horizontal = 0,
    horizontalLeft = 2,
    horizontalRight = 3,
    vertical = 4,
    verticalTop = 5,
    verticalBottom = 6
}

public enum WinType
{
    NotSelected = 0,
    Destroy = 1,
    WinTriple = 2,
    WinQuadruple = 3,
    WinTheFifth = 4,
    WinCrossroad = 5,
}