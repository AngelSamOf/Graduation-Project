using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellAllFieldChanceReplacement", menuName = "Scriptable Spell/Change Symbol In All Field")]
public class SpellAllFieldChanceReplacement : BaseSpell
{
    [SerializeField] private string _symbolID;
    [SerializeField] private int _changeReplace;

    public override void Implement()
    {
        BattleStorage storage = BattleStorage.GetInstance();

        // Меняем символ с шансов все символы на поле
        ChangeSymbol(storage);
    }

    private void ChangeSymbol(BattleStorage storage)
    {
        Field field = storage.FieldData.Field;
        FieldSymbol symbolWeigth = storage.FieldData.Symbols.Find(
            symbol => symbol.Symbol.ID == _symbolID
        ) ?? throw new Exception("[SpellChangeSymbol] not found target symbol data!");
        SymbolObject newSymbolData = symbolWeigth.Symbol;

        for (int x = 0; x < field.SizeX; x++)
        {
            for (int y = 0; y < field.SizeY; y++)
            {
                // Пропуск символа, если это тот же или уничтоженный
                SymbolBase symbol = storage.SymbolMap[x, y];
                if (symbol.SymbolData == null)
                {
                    continue;
                }
                if (symbol.SymbolData.ID == _symbolID)
                {
                    continue;
                }

                // Проверяем символ на замену
                int randomValue = UnityEngine.Random.Range(0, 100);
                if (randomValue > _changeReplace)
                {
                    continue;
                }

                symbol.SetSymbolData(newSymbolData);
            }
        }
    }
}