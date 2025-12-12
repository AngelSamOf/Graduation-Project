using UnityEngine;

public class ContractInitTexture
{
    private static ContractInitTexture _instance;
    private ContractInitTexture() { }
    private BattleStorage _storage;

    public static ContractInitTexture GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Init Texture\": start Implement");
        // Инициализация данных
        _storage = BattleStorage.GetInstance();

        InitBackground();

        Debug.Log("Contract \"Init Texture\": end Implement");
    }

    private void InitBackground()
    {
        GameObject background = new("bakcground");
        background.transform.position = new Vector3(0, 0, 5);
        SpriteRenderer bgSpriteRenderer = background.AddComponent<SpriteRenderer>();
        bgSpriteRenderer.sprite = _storage.FieldData.Textures.Background;
    }
}