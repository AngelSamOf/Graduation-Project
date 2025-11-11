using UnityEngine;

public class CellBase : MonoBehaviour
{
    public Transform NodeTransform => _transform;
    private Transform _transform;

    public CellPosition Position => _position;
    private CellPosition _position;

    public void Init(CellPosition position)
    {
        _position = position;
        _transform = this.transform;
    }
}
