using System.Collections.Generic;
using Unity.VisualScripting;
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

        // Получение данных
        List<CellBase> cellList = this.GenerateField();
        List<SymbolBase> symbolsList = this.GenerateSymbol(cellList);

        //Сохранение данных
        CellBase[,] fieldMap = ListToArray(cellList);
        _storage.SetFieldMap(fieldMap);
        SymbolBase[,] symbolMap = ListToArray(symbolsList);
        _storage.SetSymbolMap(symbolMap);
        Debug.Log("Contract \"Init Field\": end Implement");
    }

    private List<CellBase> GenerateField()
    {
        List<CellBase> cellList = new();

        for (int y = 0; y < _storage.FieldData.SizeY; y++)
        {
            for (int x = 0; x < _storage.FieldData.SizeX; x++)
            {
                GameObject cell = new($"{x}-{y}");
                cell.transform.position = new(
                    x * _storage.FieldData.StepX,
                    y * _storage.FieldData.StepY * -1
                );
                cell.transform.SetParent(_storage.Components.FieldContainer);
                CellBase cellBase = cell.AddComponent<CellBase>();
                cellBase.Init(new(x, y), _storage.FieldData.CellTexture);
                cellList.Add(cellBase);
            }
        }
        _storage.Components.FieldContainer.position = new Vector3(
            _storage.FieldData.SizeX / 2 * -1,
            _storage.FieldData.SizeY / 2
        );
        _storage.Components.SymbolContainer.position = new Vector3(
            _storage.FieldData.SizeX / 2 * -1,
            _storage.FieldData.SizeY / 2
        );
        return cellList;
    }

    private List<SymbolBase> GenerateSymbol(List<CellBase> field)
    {
        List<SymbolBase> symbolList = new();
        foreach (CellBase cell in field)
        {
            SymbolObject symbolData = SymbolMethods.GetRandomSymbol(_storage);
            GameObject symbol = new("Symbol");
            symbol.transform.position = new(
                cell.transform.position.x,
                cell.transform.position.y
            );
            symbol.transform.SetParent(_storage.Components.SymbolContainer);
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