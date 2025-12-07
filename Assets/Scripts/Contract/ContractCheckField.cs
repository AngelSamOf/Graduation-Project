using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class ContractCheckField
{
    private static ContractCheckField _instance;
    private ContractCheckField() { }
    private BattleStorage _storage;
    private List<WinCombination> _winList;

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
        _winList = new();

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
        // Просчёт победных комбинаций (выставление типа)
        CalculateWin();
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
                WinType.NotSelected
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
                WinType.NotSelected
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

        _winList.Add(combination);
    }

    private void CalculateWin()
    {
        for (int i = 0; i < _winList.Count; i++)
        {
            WinCombination currentWin = _winList[i];
            // Пропуск если победный тип уже выставлен
            if (currentWin.WinType != WinType.NotSelected)
            {
                continue;
            }

            // Поиск пересечения
            for (int j = i + 1; j < _winList.Count; j++)
            {
                if (WinHasIntersection(currentWin, _winList[j]))
                {
                    currentWin.SetWinType(WinType.WinCrossroad);
                    _winList[j].SetWinType(WinType.Destroy);
                    _storage.AddWin(_winList[j]);
                    continue;
                }
            }

            // Выставление другого WinType
            if (currentWin.WinType == WinType.NotSelected)
            {
                switch (currentWin.Positions.Count)
                {
                    case 3:
                        currentWin.SetWinType(WinType.WinTriple);
                        break;
                    case 4:
                        currentWin.SetWinType(WinType.WinQuadruple);
                        break;
                    default:
                        currentWin.SetWinType(WinType.WinTheFifth);
                        break;
                }
            }

            _storage.AddWin(currentWin);
        }
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

    private bool WinHasIntersection(WinCombination a, WinCombination b)
    {
        return a.Positions.Any(posA =>
            b.Positions.Any(posB => posA.X == posB.X && posA.Y == posB.Y));
    }
}