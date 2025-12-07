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

public class Constants
{
    public readonly float MoveTime = 0.5f;
    public readonly float ShiftTime = 0.2f;
    public readonly float ShiftMove = 0.2f;
    public readonly float DropStartPosY = 5f;
    public readonly float CharacterShift = 3f;
    public readonly Vector2 CharacterColliderSize = new(2f, 2f);
    public readonly float CharacterSubContainerShift = 1.5f;
    public readonly float IconShift = 0.6f;
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