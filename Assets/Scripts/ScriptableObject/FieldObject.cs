using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class FieldSymbol
{
    public float Weight => _weight;
    [SerializeField] private float _weight = 1;
    public SymbolObject Symbol => _symbol;
    [SerializeField] private SymbolObject _symbol;

}

[CreateAssetMenu(fileName = "FieldObject", menuName = "Scriptable Object/Field Object")]
public class FieldObject : ScriptableObject
{
    public int SizeX => _sizeX;
    [SerializeField] private int _sizeX = 10;
    public int SizeY => _sizeY;
    [SerializeField] private int _sizeY = 10;
    public float StepX => _stepX;
    [SerializeField] private float _stepX = 0;
    public float StepY => _stepY;
    [SerializeField] private float _stepY = 0;
    public List<FieldSymbol> Symbols => _symbols;
    [SerializeField] private List<FieldSymbol> _symbols = new();
    public Component Cell => _cell;
    [SerializeField] private Component _cell;
}