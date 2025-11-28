using System.Threading.Tasks;
using UnityEngine;

public class EntryGate : MonoBehaviour
{
    [SerializeField] protected DefaultComponents _components;
    [SerializeField] protected FieldObject _field;
    protected BattleStorage _storage;

    async void Start()
    {
        // Инициализация
        // Инициализация хранилища
        _storage = BattleStorage.GetInstance();
        _storage.SetComponents(_components);
        _storage.SetFieldData(_field);

        // Проверка данных на валидность
        ContractCheckData.GetInstance().Implement();

        // Инициализация поля
        ContractInitField.GetInstance().Implement();
        // Проверка сгенерированного поля на победные комбинации
        await CheckWinsCircle();

        // Подписки на события
        // Подписка на перемещение символа на поле
        EventEmitter.MoveSymbol += SymbolMove;
    }

    private async void SymbolMove(Direction direction, SymbolBase symbol)
    {
        // Перемещение символа
        await ContractSymbolMove.GetInstance().Implement(direction, symbol);
        await CheckWinsCircle();
    }

    private async Task CheckWinsCircle()
    {
        do
        {
            // Проверка на победные комбинации
            ContractCheckField.GetInstance().Implement();
            if (_storage.Wins.Count == 0)
                break;

            // Удаление всех победных комбинаций
            await ContractCascade.GetInstance().Implement();
            _storage.ClearWins();
        } while (true);
    }
}
