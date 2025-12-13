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

        // Инициализация данных
        BattleStorage storage = BattleStorage.GetInstance();

        // Задний фон
        GameObject bg = new("bg-lose-popup");
        bg.transform.position = new(0, 0, -4f);
        bg.transform.localScale = new(1000, 1000);
        SpriteRenderer bgRenderer = bg.AddComponent<SpriteRenderer>();
        bgRenderer.sprite = storage.FieldData.Textures.Fill;
        BoxCollider2D bgCollider = bg.AddComponent<BoxCollider2D>();
        bgCollider.size = new(1.0f, 1.0f);
        bg.AddComponent<ReturnToMainMenu>();

        // Popup
        GameObject popup = new("lose-popup");
        popup.transform.position = new(0, 0, -5f);
        SpriteRenderer popupRenderer = popup.AddComponent<SpriteRenderer>();
        popupRenderer.sprite = storage.FieldData.Textures.LosePopup;

        Debug.Log("Contract \"Lose Game\": end Implement");
    }
}