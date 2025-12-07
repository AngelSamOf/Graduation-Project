using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterPlayer", menuName = "Scriptable Character/Player")]
public class CharacterPlayer : BaseCharacter
{
    public List<BaseSpell> Spells => _spells;
    [SerializeField] private List<BaseSpell> _spells;

    public BaseSpell PassiveSpell => _passiveSpell;
    [SerializeField] private BaseSpell _passiveSpell;
}