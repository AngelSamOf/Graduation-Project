using UnityEngine;

[CreateAssetMenu(fileName = "EnergyObject", menuName = "Scriptable Object/Energy Object")]
public class EnergyObject : ScriptableObject
{
    public Sprite TextureFill => _textureFill;
    [SerializeField] private Sprite _textureFill;
    public Sprite TextureEmpty => _textureEmpty;
    [SerializeField] private Sprite _textureEmpty;
}