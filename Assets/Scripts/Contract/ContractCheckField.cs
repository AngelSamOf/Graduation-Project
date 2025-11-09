using System.Collections.Generic;
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
        Debug.Log("Contract \"CheckField\": end Implement");
    }
}