using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HPComponent : MonoBehaviour
{
    private HPObject _data;
    private SpriteRenderer _texture;

    public bool State => _state;
    private bool _state;

    public void Init(HPObject data, bool state = true)
    {
        _texture = GetComponent<SpriteRenderer>();
        _data = data;

        SetState(state);
    }

    public void SetState(bool state)
    {
        _state = state;
        _texture.sprite = state ? _data.TextureFill : _data.TextureEmpty;
    }
}
