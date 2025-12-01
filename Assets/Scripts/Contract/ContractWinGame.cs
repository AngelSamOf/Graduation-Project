using UnityEngine;

public class ContractWinGame
{
    private static ContractWinGame _instance;
    private ContractWinGame() { }

    public static ContractWinGame GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Win Game\": start Implement");

        // Логика на заверщение игры

        Debug.Log("Contract \"Win Game\": end Implement");
    }
}