using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FieldObject", menuName = "Scriptable Object/Field Object")]
public class FieldObject : ScriptableObject
{
    // Параметры поля
    public Field Field => _field;
    [SerializeField] private Field _field = new();

    // Клетка
    public CellObject Cell => _cell;
    [SerializeField] private CellObject _cell;

    // Символы
    public List<FieldSymbol> Symbols => _symbols;
    [SerializeField] private List<FieldSymbol> _symbols = new();

    // Персонажи
    public List<CharacterPlayer> PlayerCharacter => _playerCharacter;
    [SerializeField] private List<CharacterPlayer> _playerCharacter = new();
    public List<CharacterEnemy> EnemyCharacter => _enemyCharacter;
    [SerializeField] private List<CharacterEnemy> _enemyCharacter = new();

    // Условия победы
    public Wins Wins => _wins;
    [SerializeField] private Wins _wins = new();
}

[Serializable]
public class Field
{
    public int SizeX => _sizeX;
    [SerializeField] private int _sizeX = 10;
    public int SizeY => _sizeY;
    [SerializeField] private int _sizeY = 10;
    public float StepX => _stepX;
    [SerializeField] private float _stepX = 0;
    public float StepY => _stepY;
    [SerializeField] private float _stepY = 0;
}

[Serializable]
public class Wins
{
    // Победа по количеству ходов
    public bool IsStepWin => _isStepWin;
    [SerializeField] private bool _isStepWin = false;
    public int StepLimit => _stepLimit;
    [SerializeField] private int _stepLimit = 0;
    // Победа по набору символов
    public bool IsSymbolWin => _isSymbolWin;
    [SerializeField] private bool _isSymbolWin = false;
    public List<SymbolCondition> SymbolConditions => _symbolConditions;
    [SerializeField] private List<SymbolCondition> _symbolConditions = new();
    // Победа по каскадам
    public bool IsCombinationWin => _isCombinatiobWin;
    [SerializeField] private bool _isCombinatiobWin = false;
    public List<CombinationCondition> CombinationConditions => _combinationConditions;
    [SerializeField] private List<CombinationCondition> _combinationConditions = new();
}

[Serializable]
public class FieldSymbol
{
    public float Weight => _weight;
    [SerializeField] private float _weight = 1;
    public SymbolObject Symbol => _symbol;
    [SerializeField] private SymbolObject _symbol;

}

[Serializable]
public class SymbolCondition : Condition
{
    public string SymbolID => _symbolID;
    [SerializeField] private string _symbolID;
}

[Serializable]
public class CombinationCondition : Condition
{
    public WinType CombinationType => _combinationType;
    [SerializeField] private WinType _combinationType = WinType.NotSelected;

}

public class Condition
{
    public int Count => _count;
    [SerializeField] private int _count = 0;
    public bool IsComplete => _isComplete;
    private bool _isComplete = false;
    public void Complete()
    {
        _isComplete = true;
    }
}