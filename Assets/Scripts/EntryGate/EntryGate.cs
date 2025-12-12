using System.Threading.Tasks;
using UnityEngine;

public class EntryGate : MonoBehaviour
{
    [SerializeField] protected FieldObject _field;
    protected BattleStorage _storage;

    private ContractSymbolMove _contractSymbolMove;
    private ContractCheckField _contractCheckField;
    private ContractCombination _contractCombination;
    private ContractShowSpell _contractShowSpell;

    async void Start()
    {
        // Инициализация
        // Инициализация хранилища
        _storage = BattleStorage.GetInstance();
        _storage.SetFieldData(_field);

        // Проверка данных на валидность
        ContractCheckData.GetInstance().Implement();

        // Инициализация прочих контрактов
        _contractSymbolMove = ContractSymbolMove.GetInstance();
        _contractCheckField = ContractCheckField.GetInstance();
        _contractCombination = ContractCombination.GetInstance();
        _contractShowSpell = ContractShowSpell.GetInstance();

        // Инициализация победных условий
        ContractInitWinCondition.GetInstance().Implement();
        // Инициализация текстур
        ContractInitTexture.GetInstance().Implement();

        // Инициализация поля
        ContractInitField.GetInstance().Implement();
        // Инициализация персонажей
        ContractInitCharacter.GetInstance().Implement();

        // Проверка сгенерированного поля на победные комбинации
        await CheckWinsCircle();

        // Подписки на события
        // Подписка на перемещение символа на поле
        EventEmitter.MoveSymbol += SymbolMove;
        // Отображение способностей при нажатии на персонажа
        EventEmitter.ClickCharacter += _contractShowSpell.Implement;
    }

    private async void SymbolMove(Direction direction, SymbolBase symbol)
    {
        // Перемещение символа
        await _contractSymbolMove.Implement(direction, symbol);
        await CheckWinsCircle();
    }

    private async Task CheckWinsCircle()
    {
        do
        {
            // Проверка на победные комбинации
            _contractCheckField.Implement();
            if (_storage.Wins.Count == 0)
                break;

            // Удаление всех победных комбинаций
            await _contractCombination.Implement();
            _storage.ClearWins();
        } while (true);
    }
}
