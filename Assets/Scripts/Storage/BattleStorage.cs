using System;
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

    public Transform[,] FieldMap => _fieldMap;
    private Transform[,] _fieldMap;
    public void SetFieldMap(Transform[,] data)
    {
        _fieldMap = data;
    }

    public ISymbol[,] SymbolMap => _symbolMap;
    private ISymbol[,] _symbolMap;
    public void SetSymbolMap(ISymbol[,] data)
    {
        _symbolMap = data;
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