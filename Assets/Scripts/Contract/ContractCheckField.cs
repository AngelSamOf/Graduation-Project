using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ContractCheckField
{
    private static ContractCheckField _instance;
    private ContractCheckField() { }
    private BattleStorage storage;

    public static ContractCheckField GetInstance()
    {
        if (_instance == null)
        {
            _instance = new();
        }
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"CheckField\": start Implement");
        // Инициализация данных
        storage = BattleStorage.GetInstance();

        // Начало просчёта (горизонтальных)
        for (int x = 0; x < storage.FieldData.SizeX; x++)
        {
            CheckCell(x, 0, null, Direction.vertical);
        }
        // Начало просчёта (вертикальных)
        for (int y = 0; y < storage.FieldData.SizeY; y++)
        {
            CheckCell(0, y, null, Direction.horizontal);
        }
        // Вывод всех победных комбинаций
        PrintWinCombination();
        Debug.Log("Contract \"CheckField\": end Implement");
    }

    private void CheckCell(
        int x,
        int y,
        WinCombination combination,
        Direction direction
    )
    {
        // Проверка что нет выхода за пределы поля
        if (
            x >= storage.FieldData.SizeX ||
            y >= storage.FieldData.SizeY
        )
        {
            SaveCombination(combination);
            return;
        }

        SymbolBase symbol = storage.SymbolMap[x, y];
        string symbolID = symbol.SymbolData.ID;
        // Старт новой комбинации
        if (combination == null)
        {
            WinCombination newCombination = new(symbolID, new() { new(x, y) });
            NextSymbol(x, y, newCombination, direction);
            return;
        }

        // Проверка входит ли символ в комбинацию
        if (combination.ID == symbolID)
        {
            combination.Positions.Add(new(x, y));
            NextSymbol(x, y, combination, direction);
        }
        else
        {
            SaveCombination(combination);
            WinCombination newCombination = new(symbolID, new() { new(x, y) });
            NextSymbol(x, y, newCombination, direction);
        }
    }

    private void NextSymbol(
        int x,
        int y,
        WinCombination combination,
        Direction direction
    )
    {
        if (direction == Direction.horizontal)
        {
            CheckCell(x + 1, y, combination, direction);
        }
        else
        {
            CheckCell(x, y + 1, combination, direction);
        }
    }

    private void SaveCombination(WinCombination combination)
    {
        if (combination.Positions.Count < 3)
        {
            return;
        }
        storage.AddWin(combination);
    }

    private void PrintWinCombination()
    {
        foreach (WinCombination win in storage.Wins)
        {
            StringBuilder str = new();
            foreach (CellPosition symbol in win.Positions)
            {
                str.Append($"{symbol.X}:{symbol.Y},");
            }
            Debug.Log($"{win.ID}: {str}");
        }
    }

    private enum Direction
    {
        horizontal = 0,
        vertical = 1
    }
}