using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContractCheckData
{
    private static ContractCheckData _instance;
    private ContractCheckData() { }
    private BattleStorage storage;

    public static ContractCheckData GetInstance()
    {
        if (_instance == null)
        {
            _instance = new();
        }
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Check Data\": start Implement");
        // Инициализация данных
        storage = BattleStorage.GetInstance();

        SymbolWeightCheck();


        Debug.Log("Contract \"Check Data\": end Implement");
    }

    private void SymbolWeightCheck()
    {
        float weight = 0f;
        foreach (FieldSymbol symbol in storage.FieldData.Symbols)
        {
            weight += symbol.Weight;
        }

        if (weight != 1)
        {
            throw new Exception("Symbol total weight is not equal to zero");
        }
    }
}