using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class SymbolBase :
    MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    public SymbolObject SymbolData => _symbolData;
    protected SymbolObject _symbolData;

    public CellPosition Position => _position;
    protected CellPosition _position;

    protected bool _isMove = false;
    protected Vector3 _startPos;
    protected SpriteRenderer _spriteRendere;
    protected BoxCollider2D _collider;

    public void Init()
    {
        // Получение компонентов с объекта
        _spriteRendere = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();

        // Инициализация компонента
        UpdateTexture();
        _collider.size = new(1.0f, 1.0f);
    }

    public void SetSymbolData(SymbolObject data)
    {
        _symbolData = data;
    }

    public void SetPosition(int x, int y)
    {
        if (_position == null)
        {
            _position = new(x, y);
        }
        else
        {
            _position.UpdatePos(x, y);
        }
    }
    public void SetPosition(CellPosition position)
    {
        _position = position.Clone();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isMove = true;
        _startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isMove)
        {
            return;
        }

        // Проверка на клик
        if (_startPos == new Vector3(eventData.position.x, eventData.position.y))
        {
            return;
        }

        Direction direction = CheckDirection(_startPos, eventData.position);
        _isMove = false;
        // Вызов события на обновление позиции
        EventEmitter.MoveSymbol(direction, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMove = false;
    }

    public void MoveSymbol(float time, Vector3 position)
    {
        _isMove = true;
        this.AddComponent<MoveAction>()
            .MoveWithTime(time, position);
    }

    public void DestroySymbol()
    {
        _symbolData = null;
        _isMove = true;
        UpdateTexture();
    }

    public void UpdateTexture()
    {
        _spriteRendere.sprite = _symbolData == null ?
            null :
            _symbolData.Texture;
    }

    public void MoveSymbolAndReturn(
        float time,
        Vector3 positionTarget
    )
    {
        Vector3 startPosition = transform.localPosition;

        this.AddComponent<MoveAction>()
            .MoveWithTime(time, positionTarget, () =>
            {
                this.AddComponent<MoveAction>()
                .MoveWithTime(time, startPosition);
            }
        );
    }

    private Direction CheckDirection(Vector3 startPos, Vector3 endPos)
    {
        float differenceX = endPos.x - startPos.x;
        float differenceY = endPos.y - startPos.y;

        if (Math.Abs(differenceX) > Math.Abs(differenceY))
        {
            return differenceX > 0 ?
                Direction.horizontalRight :
                Direction.horizontalLeft;
        }
        else
        {
            return differenceY > 0 ?
                Direction.vertivalTop :
                Direction.verticalBottom;
        }
    }
}
