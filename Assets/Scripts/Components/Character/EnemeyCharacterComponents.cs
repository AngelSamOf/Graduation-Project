using UnityEngine;

public class EnemeyCharacterComponents : CharacterComponent
{
    public new CharacterEnemy Data => _data;
    protected new CharacterEnemy _data;

    public override void Init(BaseCharacter data)
    {
        base.Init(data);

        // Костыль
        // Переопределение data
        _data = (CharacterEnemy)data;

        // Подписка на события
        EventEmitter.WinCombination += CheckSpell;
    }

    public void CheckSpell(WinCombination win)
    {
        // Проверка условий
        if (win.ID != _data.Symbol.ID || _currentEnergy != _data.Energy)
        {
            return;
        }

        _data.Spell.Implement();
        RemoveEnergy(_data.Spell.Cost);
    }
}