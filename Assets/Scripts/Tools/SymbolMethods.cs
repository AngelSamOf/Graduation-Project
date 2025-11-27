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
        (
            storage.SymbolMap[firstSymbol.Position.X, firstSymbol.Position.Y],
            storage.SymbolMap[secondSymbol.Position.X, secondSymbol.Position.Y]
        ) =
        (
            storage.SymbolMap[secondSymbol.Position.X, secondSymbol.Position.Y],
            storage.SymbolMap[firstSymbol.Position.X, firstSymbol.Position.Y]
        );
        secondSymbol.SetPosition(firstSymbol.Position);
        firstSymbol.SetPosition(secondSymbol.Position);
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