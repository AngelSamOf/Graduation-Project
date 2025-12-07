using UnityEngine;

public abstract class BaseSpell : ScriptableObject
{
    public Sprite Texture => _texture;
    [SerializeField] private Sprite _texture;
    public int Cost => _cost;
    [SerializeField] private int _cost;

    public abstract void Implement();
}