using UnityEngine;

public class EntryGate : MonoBehaviour
{
    [SerializeField] protected DefaulComponents _components;
    [SerializeField] protected FieldObject _field;

    void Start()
    {
        // Инициализация хранилища
        BattleStorage storage = BattleStorage.GetInstance();
        storage.SetComponents(_components);
        storage.SetFieldData(_field);
        // Инициализация поля
        ContractInitField fieldInit = ContractInitField.GetInstance();
        fieldInit.Implement();
        // Проверка сгенерированного поля на победные комбинации
        ContractCheckField checkField = ContractCheckField.GetInstance();
        checkField.Implement();
    }
}
