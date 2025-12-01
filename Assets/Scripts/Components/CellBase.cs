using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellBase : MonoBehaviour
{
    public CellPosition Position => _position;
    private CellPosition _position;

    protected SpriteRenderer _spriteRenderer;

    public void Init(CellPosition position, Sprite sprite)
    {
        _position = position;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            _spriteRenderer.sprite = sprite;
        }
    }
}
