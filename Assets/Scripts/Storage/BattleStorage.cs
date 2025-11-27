using System.Collections.Generic;

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

