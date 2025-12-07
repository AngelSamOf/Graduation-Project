using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterObject", menuName = "Scriptable Object/Character Object")]
public class CharacterObject : ScriptableObject
{
    public string Name => _name;
    [SerializeField] private string _name;
    public int HP => _hp;
    [SerializeField] private int _hp;
    public int Energy => _energy;
    [SerializeField] private int _energy;
    public SymbolObject Symbol => _symbol;
    [SerializeField] private SymbolObject _symbol;
    public Sprite Texure => _texture;
    [SerializeField] private Sprite _texture;
}
