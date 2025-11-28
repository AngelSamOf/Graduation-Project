using UnityEngine;
using System.Collections.Generic;

static class SymbolMethods
{
    // Установка новых позиций символов
    static public void SwapPosition(
        BattleStorage storage,
        SymbolBase firstSymbol,
        SymbolBase secondSymbol
    )
    {
        CellPosition firstPos = firstSymbol.Position.Clone();
        CellPosition secondPos = secondSymbol.Position.Clone();

        // Смена позиций в массиве
        (
            storage.SymbolMap[secondPos.X, secondPos.Y],
            storage.SymbolMap[firstPos.X, firstPos.Y]
        ) = (
            storage.SymbolMap[firstPos.X, firstPos.Y],
            storage.SymbolMap[secondPos.X, secondPos.Y]
        );

        // Установка позиций у самих символов
        firstSymbol.SetPosition(secondPos);
        secondSymbol.SetPosition(firstPos);
    }

    static public SymbolObject GetRandomSymbol(BattleStorage storage)
    {
        List<FieldSymbol> symbols = storage.FieldData.Symbols;
        float targetWeight = Random.value;
        for (int i = 0; i < symbols.Count; i++)
        {
            if (symbols[i].Weight >= targetWeight)
            {
                return symbols[i].Symbol;
            }
            else
            {
                targetWeight -= symbols[i].Weight;
            }
        }
        return null;
    }
}