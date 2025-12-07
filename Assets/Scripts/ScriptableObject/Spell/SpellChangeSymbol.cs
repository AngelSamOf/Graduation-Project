using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellChangeSymbol", menuName = "Scriptable Spell/Change Symbol")]
public class SpellChangeSymbol : BaseSpell
{
    [SerializeField] private string _symbolIDFirst;
    [SerializeField] private string _symbolIDSecond;

    public override async void Implement()
    {
        BattleStorage storage = BattleStorage.GetInstance();

        // Меняем символ A на символ B
        ChangeSymbol(storage);

        // По кругу проверяем победные символы
        await CheckWinsCircle(storage);
    }

    private void ChangeSymbol(BattleStorage storage)
    {
        Field field = storage.FieldData.Field;
        FieldSymbol symbolWeigth = storage.FieldData.Symbols.Find(
            symbol => symbol.Symbol.ID == _symbolIDSecond
        );
        // Вывод ошибки если данные не найдены
        if (symbolWeigth == null)
        {
            throw new Exception("[SpellChangeSymbol] not found target symbol data!");
        }
        SymbolObject newSymbolData = symbolWeigth.Symbol;

        for (int x = 0; x < field.SizeX; x++)
        {
            for (int y = 0; y < field.SizeY; y++)
            {
                // Проверяем символ на ID
                SymbolBase symbol = storage.SymbolMap[x, y];
                if (symbol.SymbolData.ID != _symbolIDFirst)
                {
                    continue;
                }

                symbol.SetSymbolData(newSymbolData);
            }
        }
    }

    private async Task CheckWinsCircle(BattleStorage storage)
    {
        ContractCheckField contractCheckField = ContractCheckField.GetInstance();
        ContractCombination contractCombination = ContractCombination.GetInstance();


        do
        {
            contractCheckField.Implement();
            if (storage.Wins.Count == 0)
            {
                break;
            }

            await contractCombination.Implement();
            storage.ClearWins();
        } while (true);
    }
}