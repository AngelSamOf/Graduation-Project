using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContractCascade
{
    private static ContractCascade _instance;
    private ContractCascade() { }
    private BattleStorage storage;

    public static ContractCascade GetInstance()
    {
        if (_instance == null)
        {
            _instance = new();
        }
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Cascade\": start Implement");
        // Инициализация данных
        storage = BattleStorage.GetInstance();

        // Проверка что победные комбинации есть
        if (storage.Wins.Count == 0)
        {
            return;
        }

        // Уничтожение победивших символов
        List<CellPosition>[] posStack = DestroySymbol();
        foreach (List<CellPosition> positions in posStack)
        {
            CascadeSymbol(positions);
        }
        // Выпадение новых символов
        for (int x = 0; x < storage.FieldData.SizeX; x++)
        {
            DropNewSymbol(x);
        }

        Debug.Log("Contract \"Cascade\": end Implement");
    }

    private List<CellPosition>[] DestroySymbol()
    {
        List<CellPosition>[] posStack = new List<CellPosition>[storage.FieldData.SizeX];
        for (int i = 0; i < posStack.Length; i++)
        {
            posStack[i] = new();
        }

        foreach (WinCombination win in storage.Wins)
        {
            // Игнорируем не победные комбинации
            if (win.WinType != WinType.Win)
            {
                continue;
            }

            foreach (CellPosition position in win.Positions)
            {
                // Уничтожение символа и сохранение его позиции
                SymbolBase symbol = storage.SymbolMap[position.X, position.Y];
                if (symbol.SymbolData)
                {
                    symbol.DestroySymbol();
                    posStack[position.X].Add(position);
                }
            }
        }

        for (int i = 0; i < posStack.Length; i++)
        {
            posStack[i].Sort(
                (first, second) => second.Y.CompareTo(first.Y)
            );
        }
        return posStack;
    }

    private void CascadeSymbol(List<CellPosition> positions)
    {
        // Проверка что победные символы есть в столбце
        if (positions.Count == 0)
        {
            return;
        }

        // Смещение всех выше стоящих символов на позицию вниз
        int x = positions.First().X;
        int startY = positions.First().Y;
        int symbolCount = startY - positions.Count;
        for (int i = 0; i <= symbolCount; i++)
        {
            SymbolBase targetSymbol = storage.SymbolMap[x, startY];
            for (int y = startY - 1; y >= 0; y--)
            {
                SymbolBase symbol = storage.SymbolMap[x, y];
                // Скип пустого символа
                if (!symbol.SymbolData)
                {
                    continue;
                }

                // Смена позиций символов в массиве
                SymbolMethods.SwapPosition(storage, symbol, targetSymbol);

                // Запуск анимаций перемещения
                targetSymbol.MoveSymbol(
                    storage.Constants.MoveTime,
                    symbol.transform.localPosition
                );
                symbol.MoveSymbol(
                    storage.Constants.MoveTime,
                    storage.FieldMap[x, startY].transform.localPosition
                );

                startY -= 1;
                break;
            }
        }
    }

    private void DropNewSymbol(int columnIndex)
    {
        int symbolIndex = 0;
        for (int y = storage.FieldData.SizeY - 1; y >= 0; y--)
        {
            SymbolBase symbol = storage.SymbolMap[columnIndex, y];
            if (symbol.SymbolData)
            {
                continue;
            }

            Vector3 cellPosition = storage.FieldMap[columnIndex, y].transform.localPosition;

            float posX = storage.Constants.DropStartPosY
                + symbolIndex * storage.FieldData.SteapY;

            symbol.transform.localPosition = new Vector3(
                symbol.transform.localPosition.x,
                posX,
                0f
            );
            SymbolObject newSymbol = SymbolMethods.GetRandomSymbol(storage);
            symbol.SetSymbolData(newSymbol);
            symbol.UpdateTexture();

            // Запуск анимации падения
            symbol.MoveSymbol(storage.Constants.MoveTime, cellPosition);

            symbolIndex += 1;
        }
    }
}