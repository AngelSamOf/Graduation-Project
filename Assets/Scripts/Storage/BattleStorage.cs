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

    public int StepCount => _stepCount;
    private int _stepCount;
    public void ResetStepCount()
    {
        _stepCount = 0;
    }
    public void IncreaseStep()
    {
        _stepCount += 1;
    }

    public Dictionary<string, int> SymbolCount => _symbolCount;
    private Dictionary<string, int> _symbolCount = new();
    public void AddSymbolCounter(string id)
    {
        _symbolCount.Add(id, 0);
    }
    public void IncreaseSymbolCounter(string id, int count)
    {
        _symbolCount[id] += count;
    }

    public Dictionary<WinType, int> CombinationCount => _combinationCount;
    private Dictionary<WinType, int> _combinationCount = new();
    public void AddWinCounter(WinType type)
    {
        _combinationCount.Add(type, 0);
    }
    public void IncreaseWinCounter(WinType type)
    {
        _combinationCount[type] += 1;
    }

    public int FulfilledVictoryConditions => _fulfilledVictoryConditions;
    private int _fulfilledVictoryConditions = 0;
    public int VictoryConditions => _victoryConditions;
    private int _victoryConditions = 0;
    public void SetVictoryCondition(int count)
    {
        _victoryConditions = count;
        _fulfilledVictoryConditions = 0;
    }
    public void IncreaseVictoryCondition()
    {
        _fulfilledVictoryConditions += 1;
    }
}

