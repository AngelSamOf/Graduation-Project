using UnityEngine;

[CreateAssetMenu(fileName = "CharacterEnemy", menuName = "Scriptable Character/Enemy")]
public class CharacterEnemy : BaseCharacter
{
    public BaseSpell Spell => _spell;
    [SerializeField] private BaseSpell _spell;
}