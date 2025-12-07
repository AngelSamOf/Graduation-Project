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

    public void Implement()
    {
        Debug.Log("Contract \"Init Field\": start Implement");
        // Инициализация данных
        _storage = BattleStorage.GetInstance();

        // Добавление на поле основных контейнеров
        (GameObject symbolContainer, GameObject cellContainer) = GenerateBaseContainer();

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

    private (GameObject symbolContainer, GameObject cellContainer) GenerateBaseContainer()
    {
        GameObject cellContainer = new("cell-container");
        GameObject symbolContainer = new("symbol-container");

        cellContainer.transform.position = new Vector3(
            _storage.FieldData.SizeX / 2 * -1,
            _storage.FieldData.SizeY / 2
        );
        symbolContainer.transform.position = new Vector3(
            _storage.FieldData.SizeX / 2 * -1,
            _storage.FieldData.SizeY / 2
        );

        return (symbolContainer, cellContainer);
    }

    private List<CellBase> GenerateField(GameObject cellContainer)
    {
        List<CellBase> cellList = new();

        for (int y = 0; y < _storage.FieldData.SizeY; y++)
        {
            for (int x = 0; x < _storage.FieldData.SizeX; x++)
            {
                GameObject cell = new($"{x}-{y}");
                cell.transform.SetParent(cellContainer.transform);
                cell.transform.localPosition = new(
                    x * _storage.FieldData.StepX,
                    y * _storage.FieldData.StepY * -1
                );
                CellBase cellBase = cell.AddComponent<CellBase>();
                cellBase.Init(new(x, y), _storage.FieldData.CellTexture);
                cellList.Add(cellBase);
            }
        }
        return cellList;
    }

    private List<SymbolBase> GenerateSymbol(List<CellBase> field, GameObject symbolContainer)
    {
        List<SymbolBase> symbolList = new();
        foreach (CellBase cell in field)
        {
            SymbolObject symbolData = SymbolMethods.GetRandomSymbol(_storage);
            GameObject symbol = new("symbol");
            symbol.transform.SetParent(symbolContainer.transform);
            symbol.transform.position = new(
                cell.transform.position.x,
                cell.transform.position.y
            );
            SymbolBase symbolBase = symbol.AddComponent<SymbolBase>();
            symbolBase.SetSymbolData(symbolData);
            symbolBase.SetPosition(cell.Position.X, cell.Position.Y);
            symbolBase.Init();
            symbolList.Add(symbolBase);
        }
        return symbolList;
    }

    private T[,] ListToArray<T>(List<T> list)
    {
        T[,] array = new T[_storage.FieldData.SizeX, _storage.FieldData.SizeY];
        int index = 0;
        for (int y = 0; y < _storage.FieldData.SizeY; y++)
        {
            for (int x = 0; x < _storage.FieldData.SizeX; x++)
            {
                array[x, y] = list[index];
                index++;
            }
        }
        return array;
    }
}