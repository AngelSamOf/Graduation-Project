using UnityEngine;

public class CellBase : MonoBehaviour
{
    public CellPosition Position => _position;
    private CellPosition _position;

    public void Init(CellPosition position)
    {
        _position = position;
    }
}
