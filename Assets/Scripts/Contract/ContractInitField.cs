using System.Collections.Generic;
using UnityEngine;

public class ContractInitField
{
    private static ContractInitField _instance;
    private ContractInitField() { }
    private BattleStorage _storage;

    public static ContractInitField GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public static void ResetInstance()
    {
        _instance = null;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Init Field\": start Implement");
        // Инициализация данных
        _storage = BattleStorage.GetInstance();

        // Добавление на поле основных контейнеров
        (GameObject symbolContainer, GameObject cellContainer) = GenerateFieldContainer();

        // Получение данных
        List<CellBase> cellList = GenerateField(cellContainer);
        List<SymbolBase> symbolsList = GenerateSymbol(cellList, symbolContainer);

        //Сохранение данных
        CellBase[,] fieldMap = ListToArray(cellList);
        _storage.SetFieldMap(fieldMap);
        SymbolBase[,] symbolMap = ListToArray(symbolsList);
        _storage.SetSymbolMap(symbolMap);
        Debug.Log("Contract \"Init Field\": end Implement");
    }

    // Создание контейнеров
    private (GameObject symbolContainer, GameObject cellContainer) GenerateFieldContainer()
    {
        GameObject cellContainer = new("cell-container");
        GameObject symbolContainer = new("symbol-container");

        Field field = _storage.FieldData.Field;
        cellContainer.transform.position = new(
            (field.SizeX * field.StepX - field.StepX) / 2 * -1,
            (field.SizeY * field.StepY - field.StepY) / 2
        );
        symbolContainer.transform.position = new(
            (field.SizeX * field.StepX - field.StepX) / 2 * -1,
            (field.SizeY * field.StepY - field.StepY) / 2
        );

        return (symbolContainer, cellContainer);
    }

    // Создание клеток и симболов
    private List<CellBase> GenerateField(GameObject container)
    {
        List<CellBase> cellList = new();
        Field field = _storage.FieldData.Field;

        for (int y = 0; y < field.SizeY; y++)
        {
            for (int x = 0; x < field.SizeX; x++)
            {
                GameObject cell = new($"{x}-{y}");
                cell.transform.SetParent(container.transform);
                cell.transform.localPosition = new(
                    x * field.StepX,
                    y * field.StepY * -1,
                    1
                );
                CellBase cellBase = cell.AddComponent<CellBase>();
                cellBase.Init(new(x, y), _storage.FieldData.Cell);
                cellList.Add(cellBase);
            }
        }
        return cellList;
    }

    private List<SymbolBase> GenerateSymbol(List<CellBase> field, GameObject container)
    {
        List<SymbolBase> symbolList = new();
        foreach (CellBase cell in field)
        {
            SymbolObject symbolData = SymbolMethods.GetRandomSymbol(_storage);
            GameObject symbol = new("symbol");
            symbol.transform.SetParent(container.transform);
            symbol.transform.position = new(
                cell.transform.position.x,
                cell.transform.position.y,
                0
            );
            SymbolBase symbolBase = symbol.AddComponent<SymbolBase>();
            symbolBase.SetPosition(cell.Position.X, cell.Position.Y);
            symbolBase.Init(symbolData);
            symbolList.Add(symbolBase);
        }
        return symbolList;
    }

    // Вспомогательный класс
    private T[,] ListToArray<T>(List<T> list)
    {
        Field field = _storage.FieldData.Field;
        T[,] array = new T[
            field.SizeX,
            field.SizeY
        ];
        int index = 0;
        for (int y = 0; y < field.SizeY; y++)
        {
            for (int x = 0; x < field.SizeX; x++)
            {
                array[x, y] = list[index];
                index++;
            }
        }
        return array;
    }
}