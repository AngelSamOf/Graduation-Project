using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContractInitField
{
    private static ContractInitField _instance;
    private ContractInitField() { }
    private BattleStorage storage;

    public static ContractInitField GetInstance()
    {
        if (_instance == null)
        {
            _instance = new();
        }
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"InitField\": start Implement");
        // Инициализация данных
        storage = BattleStorage.GetInstance();

        // Получение данных
        List<CellBase> cellList = this.GenerateField();
        List<SymbolBase> symbolsList = this.GenerateSymbol(cellList);

        //Сохранение данных
        CellBase[,] fieldMap = ListToArray(cellList);
        storage.SetFieldMap(fieldMap);
        SymbolBase[,] symbolMap = ListToArray(symbolsList);
        storage.SetSymbolMap(symbolMap);
        Debug.Log("Contract \"InitField\": end Implement");
    }

    private List<CellBase> GenerateField()
    {
        List<CellBase> cellList = new();

        for (int y = 0; y < storage.FieldData.SizeY; y++)
        {
            for (int x = 0; x < storage.FieldData.SizeX; x++)
            {

                Component cell = Object.Instantiate(
                    storage.FieldData.Cell,
                    new Vector3(
                        x * storage.FieldData.StepX,
                        y * storage.FieldData.StepY * -1
                    ),
                    Quaternion.identity
                );
                cell.transform.SetParent(storage.Components.FieldContainer);
                CellBase cellBase = cell.AddComponent<CellBase>();
                cellBase.Init(new(x, y));
                cell.name = $"{x}-{y}";
                cellList.Add(cellBase);
            }
        }
        storage.Components.FieldContainer.position = new Vector3(
            storage.FieldData.SizeX / 2 * -1,
            storage.FieldData.SizeY / 2
        );
        storage.Components.SymbolContainer.position = new Vector3(
            storage.FieldData.SizeX / 2 * -1,
            storage.FieldData.SizeY / 2
        );
        return cellList;
    }

    private List<SymbolBase> GenerateSymbol(List<CellBase> field)
    {
        List<SymbolBase> symbolList = new();
        foreach (CellBase cell in field)
        {
            SymbolObject symbolData = SymbolMethods.GetRandomSymbol(storage);
            GameObject symbol = new("Symbol");
            symbol.transform.position = new(
                cell.transform.position.x,
                cell.transform.position.y
            );
            symbol.transform.SetParent(storage.Components.SymbolContainer);
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
        T[,] array = new T[storage.FieldData.SizeX, storage.FieldData.SizeY];
        int index = 0;
        for (int y = 0; y < storage.FieldData.SizeY; y++)
        {
            for (int x = 0; x < storage.FieldData.SizeX; x++)
            {
                array[x, y] = list[index];
                index++;
            }
        }
        return array;
    }
}