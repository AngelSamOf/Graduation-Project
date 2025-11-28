using System.Text;
using UnityEngine;

public class ContractCheckField
{
    private static ContractCheckField _instance;
    private ContractCheckField() { }
    private BattleStorage _storage;

    public static ContractCheckField GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Check Field\": start Implement");
        // Инициализация данных
        _storage = BattleStorage.GetInstance();

        // Начало просчёта (горизонтальных)
        for (int x = 0; x < _storage.FieldData.SizeX; x++)
        {
            CheckCell(x, 0, null, Direction.vertical);
        }
        // Начало просчёта (вертикальных)
        for (int y = 0; y < _storage.FieldData.SizeY; y++)
        {
            CheckCell(0, y, null, Direction.horizontal);
        }
        // Вывод всех победных комбинаций
        PrintWinCombination();
        Debug.Log("Contract \"Check Field\": end Implement");
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
            x >= _storage.FieldData.SizeX ||
            y >= _storage.FieldData.SizeY
        )
        {
            SaveCombination(combination);
            return;
        }

        SymbolBase symbol = _storage.SymbolMap[x, y];
        // Проверка на пустоту
        if (symbol == null || symbol.SymbolData == null)
        {
            SaveCombination(combination);
            return;
        }
        string symbolID = symbol.SymbolData.ID;
        // Старт новой комбинации
        if (combination == null)
        {
            WinCombination newCombination = new(
                symbolID,
                new() { new(x, y) },
                WinType.Win
            );
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
            WinCombination newCombination = new(
                symbolID,
                new() { new(x, y) },
                WinType.Win
            );
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
        _storage.AddWin(combination);
    }

    private void PrintWinCombination()
    {
        foreach (WinCombination win in _storage.Wins)
        {
            StringBuilder str = new();
            foreach (CellPosition symbol in win.Positions)
            {
                str.Append($"{symbol.X}:{symbol.Y},");
            }
        }
    }
}