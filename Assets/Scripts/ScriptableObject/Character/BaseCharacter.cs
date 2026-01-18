using UnityEngine;

public class BaseCharacter : ScriptableObject
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
    public Sprite DeathTexture => _deathTexture;
    [SerializeField] private Sprite _deathTexture;

    public EnergyObject EnergyData => _energyData;
    [SerializeField] private EnergyObject _energyData;
    public HPObject HPData => _hpData;
    [SerializeField] private HPObject _hpData;
}