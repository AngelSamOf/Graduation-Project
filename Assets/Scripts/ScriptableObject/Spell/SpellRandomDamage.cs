using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpellRandomDamage", menuName = "Scriptable Spell/Random Damage")]
public class SpellRandomDamage : BaseSpell
{
    [SerializeField] private int _damage;

    public override void Implement()
    {
        BattleStorage storage = BattleStorage.GetInstance();
        List<PlayerCharacterComponent> playerCharacters = storage.PlayerCharacter;

        int characterIndex = Random.Range(0, playerCharacters.Count);
        playerCharacters[characterIndex].TakeDamage(_damage);
    }
}