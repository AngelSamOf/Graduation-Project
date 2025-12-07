using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ContractCombination
{
    private static ContractCombination _instance;
    private ContractCombination() { }
    private BattleStorage _storage;
    private bool _isImplement = false;

    public static ContractCombination GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public async Task Implement()
    {
        if (_isImplement)
        {
            Debug.Log("Contract \"Cascade\": aborted. There is already a running contract");
            return;
        }

        Debug.Log("Contract \"Cascade\": start Implement");
        _isImplement = true;
        // Инициализация данных
        _storage = BattleStorage.GetInstance();

        // Проверка что победные комбинации есть
        if (_storage.Wins.Count == 0)
        {
            return;
        }

        // Уничтожение победивших символов
        List<CellPosition>[] posStack = DestroySymbol();
        List<Task> tasks = new();
        foreach (List<CellPosition> positions in posStack)
        {
            tasks.Add(CascadeSymbol(positions));
        }
        await Task.WhenAll(tasks);
        tasks = new();
        // Выпадение новых символов
        for (int x = 0; x < _storage.FieldData.Field.SizeX; x++)
        {
            tasks.Add(DropNewSymbol(x));
        }
        await Task.WhenAll(tasks);

        Debug.Log("Contract \"Cascade\": end Implement");
        _isImplement = false;
    }

    private List<CellPosition>[] DestroySymbol()
    {
        List<CellPosition>[] posStack = new List<CellPosition>[
            _storage.FieldData.Field.SizeX
        ];
        for (int i = 0; i < posStack.Length; i++)
        {
            posStack[i] = new();
        }

        foreach (WinCombination win in _storage.Wins)
        {
            // Игнорируем не победные комбинации
            if (win.WinType == WinType.NotSelected)
            {
                continue;
            }
            Debug.Log($"win \"{win.ID}\" by type ({win.WinType})");

            foreach (CellPosition position in win.Positions)
            {
                // Уничтожение символа и сохранение его позиции
                SymbolBase symbol = _storage.SymbolMap[position.X, position.Y];
                if (symbol.SymbolData != null)
                {
                    symbol.DestroySymbol();
                    posStack[position.X].Add(position);
                }
            }
            // Генерация события на уничтожения
            EventEmitter.WinCombination.Invoke(win);
        }

        for (int i = 0; i < posStack.Length; i++)
        {
            posStack[i].Sort(
                (first, second) => second.Y.CompareTo(first.Y)
            );
        }

        return posStack;
    }

    private async Task CascadeSymbol(List<CellPosition> positions)
    {
        // Проверка что победные символы есть в столбце
        if (positions.Count == 0)
        {
            return;
        }

        List<Task> tasks = new();
        // Смещение всех выше стоящих символов на позицию вниз
        int x = positions.First().X;
        int startY = positions.First().Y;
        int symbolCount = startY - positions.Count;
        for (int i = 0; i <= symbolCount; i++)
        {
            SymbolBase targetSymbol = _storage.SymbolMap[x, startY];
            for (int y = startY - 1; y >= 0; y--)
            {
                SymbolBase symbol = _storage.SymbolMap[x, y];
                // Скип пустого символа
                if (!symbol.SymbolData)
                {
                    continue;
                }

                // Смена позиций символов в массиве
                SymbolMethods.SwapPosition(_storage, symbol, targetSymbol);

                // Запуск анимаций перемещения
                tasks.Add(targetSymbol.MoveSymbol(
                    _storage.Constants.MoveTime,
                    symbol.transform.localPosition
                ));
                tasks.Add(symbol.MoveSymbol(
                    _storage.Constants.MoveTime,
                    _storage.FieldMap[x, startY].transform.localPosition
                ));

                startY -= 1;
                break;
            }
        }

        await Task.WhenAll(tasks);
    }

    private async Task DropNewSymbol(int columnIndex)
    {
        int symbolIndex = 0;
        List<Task> tasks = new();

        for (int y = _storage.FieldData.Field.SizeY - 1; y >= 0; y--)
        {
            SymbolBase symbol = _storage.SymbolMap[columnIndex, y];
            if (symbol.SymbolData)
            {
                continue;
            }

            Vector3 cellPosition = _storage.FieldMap[columnIndex, y].transform.localPosition;

            float posX = _storage.Constants.DropStartPosY
                + symbolIndex * _storage.FieldData.Field.StepY;

            symbol.transform.localPosition = new Vector3(
                symbol.transform.localPosition.x,
                posX,
                0f
            );
            SymbolObject newSymbol = SymbolMethods.GetRandomSymbol(_storage);
            symbol.SetSymbolData(newSymbol);

            // Запуск анимации падения
            tasks.Add(symbol.MoveSymbol(
                _storage.Constants.MoveTime,
                cellPosition
            ));

            symbolIndex += 1;
        }

        await Task.WhenAll(tasks);
    }
}