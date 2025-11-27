using UnityEngine;

public class EntryGate : MonoBehaviour
{
    [SerializeField] protected DefaulComponents _components;
    [SerializeField] protected FieldObject _field;
    protected BattleStorage _storage;

    void Start()
    {
        // Инициализация
        // Инициализация хранилища
        _storage = BattleStorage.GetInstance();
        _storage.SetComponents(_components);
        _storage.SetFieldData(_field);
        // Инициализация поля
        ContractInitField.GetInstance().Implement();
        // Проверка сгенерированного поля на победные комбинации
        ContractCheckField.GetInstance().Implement();
        // Удаление победных комбинаций
        ContractCascade.GetInstance().Implement();

        // Подписки на события
        // Подписка на перемещение символа на поле
        EventEmitter.MoveSymbol = SymbolMove;
    }

    private void SymbolMove(Direction direction, SymbolBase symbol)
    {
        // Перемещение символа
        ContractSymbolMove.GetInstance().Implement(direction, symbol);
        // Проверка на победные комбинации
        ContractCheckField.GetInstance().Implement();
        // Удаление всех победных комбинаций
        ContractCascade.GetInstance().Implement();
        _storage.ClearWins();
    }
}
