using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnergyComponent : MonoBehaviour
{
    private EnergyObject _data;
    private SpriteRenderer _texture;

    public bool State => _state;
    private bool _state;

    public void Init(EnergyObject data, bool state = false)
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
