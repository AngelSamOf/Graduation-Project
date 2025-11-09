using UnityEngine;

[CreateAssetMenu(fileName = "SymbolObject", menuName = "Scriptable Object/Symbol Object")]
public class SymbolObject : ScriptableObject
{
    public string ID => _id;
    [SerializeField] private string _id;
    public Component Prefab => _prefab;
    [SerializeField] private Component _prefab;
}