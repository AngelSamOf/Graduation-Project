using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleStorage
{
    private static BattleStorage _instance;
    private BattleStorage() { }
    public static BattleStorage GetInstance()
    {
        if (_instance == null)
        {
            _instance = new();
        }
        return _instance;
    }

    public readonly Constants Constants = new();

    public DefaulComponents Components => _components;
    private DefaulComponents _components;
    public void SetComponents(DefaulComponents data)
    {
        _components = data;
    }

    public FieldObject FieldData => _fieldData;
    private FieldObject _fieldData;
    public void SetFieldData(FieldObject data)
    {
        _fieldData = data;
    }

    public CellBase[,] FieldMap => _fieldMap;
    private CellBase[,] _fieldMap;
    public void SetFieldMap(CellBase[,] data)
    {
        _fieldMap = data;
    }

    public SymbolBase[,] SymbolMap => _symbolMap;
    private SymbolBase[,] _symbolMap;
    public void SetSymbolMap(SymbolBase[,] data)
    {
        _symbolMap = data;
    }

    public List<WinCombination> Wins => _wins;
    private List<WinCombination> _wins = new();
    public void AddWin(WinCombination win)
    {
        _wins.Add(win);
    }
    public void ClearWins()
    {
        _wins.Clear();
    }
}

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

    public WinCombination(
        string id,
        List<CellPosition> positions
    )
    {
        _id = id;
        _positions = positions;
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
}