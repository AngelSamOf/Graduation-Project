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

    public static void ResetInstance()
    {
        _instance = null;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Win Game\": start Implement");

        // Инициализация данных
        BattleStorage storage = BattleStorage.GetInstance();

        // Задний фон
        GameObject bg = new("bg-win-popup");
        bg.transform.position = new(0, 0, -4f);
        bg.transform.localScale = new(1000, 1000);
        SpriteRenderer bgRenderer = bg.AddComponent<SpriteRenderer>();
        bgRenderer.sprite = storage.FieldData.Textures.Fill;
        BoxCollider2D bgCollider = bg.AddComponent<BoxCollider2D>();
        bgCollider.size = new(1.0f, 1.0f);
        bg.AddComponent<ReturnToMainMenu>();

        // Popup
        GameObject popup = new("win-popup");
        popup.transform.position = new(0, 0, -5f);
        SpriteRenderer popupRenderer = popup.AddComponent<SpriteRenderer>();
        popupRenderer.sprite = storage.FieldData.Textures.WinPopup;

        Debug.Log("Contract \"Win Game\": end Implement");
    }
}