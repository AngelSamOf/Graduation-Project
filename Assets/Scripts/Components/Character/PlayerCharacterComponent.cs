using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCharacterComponent : CharacterComponent, IPointerClickHandler
{
    public new CharacterPlayer Data => _data;
    protected new CharacterPlayer _data;

    public override void Init(BaseCharacter data)
    {
        base.Init(data);

        // Костыль
        // Переопределение data
        _data = (CharacterPlayer)data;

        // Получение компонентов  с объекта
        _collider = GetComponent<BoxCollider2D>();
        _collider.size = _storage.Constants.CharacterColliderSize;

        // Подписка на события
        EventEmitter.WinCombination += CheckPassive;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventEmitter.ClickCharacter.Invoke(this);
    }

    public void CheckPassive(WinCombination win)
    {
        // Проверка условий
        if (
            win.WinType != WinType.WinCrossroad ||
            win.ID != _data.Symbol.ID
        )
        {
            return;
        }

        _data.PassiveSpell.Implement();
    }
}