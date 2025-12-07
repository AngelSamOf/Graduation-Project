using UnityEngine;

[CreateAssetMenu(fileName = "CellObject", menuName = "Scriptable Object/Cell Object")]
public class CellObject : ScriptableObject
{
    public Sprite Texture => _texture;
    [SerializeField] private Sprite _texture;
}