using UnityEngine;

public class SymbolBase : MonoBehaviour
{
    public SymbolObject SymbolData => _symbolData;
    private SymbolObject _symbolData;

    public void SetSymbolData(SymbolObject data)
    {
        _symbolData = data;
    }
}
