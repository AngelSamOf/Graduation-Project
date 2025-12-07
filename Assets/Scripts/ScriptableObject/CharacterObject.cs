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

    public EnergyObject EnergyData => _energyData;
    [SerializeField] private EnergyObject _energyData;
    public HPObject HPData => _hpData;
    [SerializeField] private HPObject _hpData;

    public List<BaseSpell> Spells => _spells;
    [SerializeField] private List<BaseSpell> _spells;

    public BaseSpell PassiveSpell => _passiveSpell;
    [SerializeField] private BaseSpell _passiveSpell;
}