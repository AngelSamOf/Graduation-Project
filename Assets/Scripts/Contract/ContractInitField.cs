using System.Collections.Generic;
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
        List<Transform> cellList = this.GenerateField();
        List<SymbolBase> symbolsList = this.GenerateSymbol(cellList);

        //Сохранение данных
        Transform[,] fieldMap = ListToArray<Transform>(cellList);
        storage.SetFieldMap(fieldMap);
        SymbolBase[,] symbolMap = ListToArray<SymbolBase>(symbolsList);
        storage.SetSymbolMap(symbolMap);
        Debug.Log("Contract \"InitField\": end Implement");
    }

    private List<Transform> GenerateField()
    {
        List<Transform> cellList = new();

        for (int y = 0; y < storage.FieldData.SizeY; y++)
        {
            for (int x = 0; x < storage.FieldData.SizeX; x++)
            {

                Component cell = Component.Instantiate(
                    storage.FieldData.Cell,
                    new Vector3(
                        x * storage.FieldData.SteapX,
                        y * storage.FieldData.SteapY * -1
                    ),
                    Quaternion.identity
                );
                cell.transform.SetParent(storage.Components.FieldContainer);
                cell.name = $"{x}-{y}";
                cellList.Add(cell.transform);
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

    private List<SymbolBase> GenerateSymbol(List<Transform> field)
    {
        List<SymbolBase> symbolList = new();
        foreach (Transform cell in field)
        {
            SymbolObject symbolData = GetSymbol(storage.FieldData.Symbols);
            Component symbol = Component.Instantiate(
                symbolData.Prefab,
                new Vector3(cell.position.x, cell.position.y),
                Quaternion.identity
            );
            symbol.transform.SetParent(storage.Components.SymbolContainer);
            SymbolBase symbolBase = symbol.GetComponent<SymbolBase>();
            symbolBase.SetSymbolData(symbolData);
            symbolList.Add(symbolBase);
        }
        return symbolList;
    }

    private SymbolObject GetSymbol(List<FieldSymbol> symbols)
    {
        float targetWeight = Random.value;
        for (int i = 0; i < symbols.Count; i++)
        {
            if (symbols[i].Weight >= targetWeight)
            {
                return symbols[i].Symbol;
            }
            else
            {
                targetWeight -= symbols[i].Weight;
            }
        }
        return null;
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