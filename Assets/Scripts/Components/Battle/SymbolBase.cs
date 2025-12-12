using System;
using System.Threading.Tasks;
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
    protected SpriteRenderer _spriteRenderer;
    protected BoxCollider2D _collider;

    // Погрешность в пикселях при нажатии
    private const float _clickThreshold = 25f;

    public void Init(SymbolObject data)
    {
        // Получение компонентов с объекта
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _collider.size = new(2.0f, 2.0f);

        // Инициализация компонента
        SetSymbolData(data);
    }

    public void SetSymbolData(SymbolObject data)
    {
        _symbolData = data;
        UpdateTexture();
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

        Vector2 end = eventData.position;
        float sqrDistance = (end - (Vector2)_startPos).sqrMagnitude;

        _isMove = false;
        // Проверка на клик
        if (sqrDistance <= _clickThreshold)
        {
            return;
        }

        Direction direction = CheckDirection(_startPos, eventData.position);
        EventEmitter.MoveSymbol?.Invoke(direction, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMove = false;
    }

    public void DestroySymbol()
    {
        _symbolData = null;
        _isMove = true;
        UpdateTexture();
    }

    public void UpdateTexture()
    {
        _spriteRenderer.sprite = _symbolData == null ?
            null :
            _symbolData.Texture;
    }

    public Task MoveSymbol(float time, Vector3 position)
    {
        _isMove = true;
        MoveAction action = this.AddComponent<MoveAction>();
        return action.MoveWithTimeAsyn(time, position);
    }

    public async Task MoveSymbolAndReturn(
        float time,
        Vector3 positionTarget
    )
    {
        _isMove = true;

        Vector3 startPosition = transform.localPosition;

        // Перемещение вперёд
        MoveAction actionForward = this.AddComponent<MoveAction>();
        await actionForward.MoveWithTimeAsyn(time, positionTarget);

        // Перемещение обратно
        MoveAction actionBackward = this.AddComponent<MoveAction>();
        await actionBackward.MoveWithTimeAsyn(time, startPosition);

        _isMove = false;
    }

    private Direction CheckDirection(Vector3 startPos, Vector3 endPos)
    {
        float differenceX = endPos.x - startPos.x;
        float differenceY = endPos.y - startPos.y;

        if (Mathf.Abs(differenceX) > Mathf.Abs(differenceY))
        {
            return differenceX > 0
                ? Direction.horizontalRight
                : Direction.horizontalLeft;
        }
        else
        {
            return differenceY > 0
                ? Direction.verticalTop
                : Direction.verticalBottom;
        }
    }
}
