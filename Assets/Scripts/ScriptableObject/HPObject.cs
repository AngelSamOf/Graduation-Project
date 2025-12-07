using UnityEngine;

[CreateAssetMenu(fileName = "HPObject", menuName = "Scriptable Object/HP Object")]
public class HPObject : ScriptableObject
{
    public Sprite TextureFill => _textureFill;
    [SerializeField] private Sprite _textureFill;
    public Sprite TextureEmpty => _textureEmpty;
    [SerializeField] private Sprite _textureEmpty;
}