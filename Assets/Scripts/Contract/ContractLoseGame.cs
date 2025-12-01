using UnityEngine;

public class ContractLoseGame
{
    private static ContractLoseGame _instance;
    private ContractLoseGame() { }

    public static ContractLoseGame GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Lose Game\": start Implement");

        // Логика на заверщение игры

        Debug.Log("Contract \"Lose Game\": end Implement");
    }
}