using UnityEngine;

[CreateAssetMenu(fileName = "SpellResetTurns", menuName = "Scriptable Spell/Reset Turns")]
public class SpellResetTurns : BaseSpell
{
    [SerializeField] private int _removeStepCount;

    public override void Implement()
    {
        BattleStorage storage = BattleStorage.GetInstance();
        int removeCount = storage.StepCount < _removeStepCount ?
            storage.StepCount :
            _removeStepCount;
        storage.RemoveStep(removeCount);
    }
}