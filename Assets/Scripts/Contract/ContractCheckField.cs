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
            CheckCell(x, 0, new(), Direction.vertical);
        }
        // Начало просчёта (вертикальных)
        for (int y = 0; y < storage.FieldData.SizeY; y++)
        {
            CheckCell(0, y, new(), Direction.horizontal);
        }
        Debug.Log("Contract \"CheckField\": end Implement");
    }

    private void CheckCell(
        int x,
        int y,
        List<CellSymbol> combination,
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
        if (combination.Count == 0)
        {
            List<CellSymbol> newCombination = new()
            {
                new(x, y, symbolID)
            };
            NextSymbol(x, y, newCombination, direction);
            return;
        }

        // if (direction == Direction.horizontal)
        // {
        //     Debug.Log($"${x}:{y} ({combination[0].ID} = {symbolID})");
        // }

        // Проверка входит ли символ в комбинацию
        if (combination[0].ID == symbolID)
        {
            combination.Add(new(x, y, symbolID));
            NextSymbol(x, y, combination, direction);
        }
        else
        {
            SaveCombination(combination);
            List<CellSymbol> newCombination = new()
            {
                new(x, y, symbolID)
            };
            NextSymbol(x, y, newCombination, direction);
        }
    }

    private void NextSymbol(
        int x,
        int y,
        List<CellSymbol> combination,
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

    private void SaveCombination(List<CellSymbol> combination)
    {
        if (combination.Count < 3)
        {
            return;
        }

        StringBuilder str = new();
        foreach (CellSymbol symbol in combination)
        {
            str.Append($"{symbol.X}:{symbol.Y},");
        }
        Debug.Log($"{combination[0].ID}: {str}");
    }

    private enum Direction
    {
        horizontal = 0,
        vertical = 1
    }

    private class CellSymbol
    {
        public int X => _x;
        private int _x;
        public int Y => _y;
        private int _y;
        public string ID => _id;
        private string _id;

        public CellSymbol(
            int x,
            int y,
            string id
        )
        {
            _x = x;
            _y = y;
            _id = id;
        }
    }
}