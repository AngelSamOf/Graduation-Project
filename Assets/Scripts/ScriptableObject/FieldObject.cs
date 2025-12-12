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

    // Заднийфон
    public Textures Textures => _textures;
    [SerializeField] private Textures _textures;

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

    // Константы
    public Constants Constants => _constants;
    [SerializeField] private Constants _constants;
}

[Serializable]
public class Textures
{
    public Sprite Background => _background;
    [SerializeField] private Sprite _background;
    public Sprite TopPanel => _topPanel;
    [SerializeField] private Sprite _topPanel;
    public Sprite BottomPanel => _bottomPanel;
    [SerializeField] private Sprite _bottomPanel;
    public Sprite IconStep => _iconStep;
    [SerializeField] private Sprite _iconStep;
    public Sprite IconEnemy => _iconEnemy;
    [SerializeField] private Sprite _iconEnemy;
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
    public SymbolObject Symbol => _symbol;
    [SerializeField] private SymbolObject _symbol;
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

[Serializable]
public class Constants
{
    public float MoveTime => _moveTime;
    [SerializeField] private float _moveTime = 0.5f;
    public float ShiftTime => _shiftTime;
    [SerializeField] private readonly float _shiftTime = 0.2f;
    public float ShiftMove => _shiftMove;
    [SerializeField] private float _shiftMove = 0.2f;
    public float DropStartPosY => _dropStartPosY;
    [SerializeField] private float _dropStartPosY = 5f;
    public float CharacterShift => _characterShift;
    [SerializeField] private float _characterShift = 2.75f;
    public Vector2 CharacterColliderSize => _characterColliderSize;
    [SerializeField] private Vector2 _characterColliderSize = new(3f, 4f);
    public float CharacterSubContainerShift => _characterSubContainerShift;
    [SerializeField] private float _characterSubContainerShift = 2.2f;
    public float IconStartShift => _iconStartShift;
    [SerializeField] private float _iconStartShift = -1f;
    public float IconShift => _iconShift;
    [SerializeField] private float _iconShift = 1f;
    public float TopPanelY => _topPanelY;
    [SerializeField] private float _topPanelY = 6.6f;
    public float CanvasPPI => _canvasPPI;
    [SerializeField] private float _canvasPPI = 72f;
    public float ConditionPoxY => _conditionPoxY;
    [SerializeField] private float _conditionPoxY = 6.1f;
    public float ConditionPoxX => _conditionPoxX;
    [SerializeField] private float _conditionPoxX = -2.5f;
    public float ConditionSpacing => _conditionSpacing;
    [SerializeField] private float _conditionSpacing = 150;
}