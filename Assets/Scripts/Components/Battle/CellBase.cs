using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellBase : MonoBehaviour
{
    public CellPosition Position => _position;
    private CellPosition _position;

    protected SpriteRenderer _spriteRenderer;

    public void Init(CellPosition position, CellObject data)
    {
        _position = position;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();

        if (data.Texture != null)
        {
            _spriteRenderer.sprite = data.Texture;
        }
    }
}
