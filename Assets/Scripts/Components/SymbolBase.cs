using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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

        MoveDirection direction = CheckDirection(_startPos, eventData.position);
        _isMove = false;

        // Запуск контракта
        ContractSymbolMove contract = ContractSymbolMove.GetInstance();
        contract.Implement(direction, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMove = false;
    }

    private MoveDirection CheckDirection(Vector3 startPos, Vector3 endPos)
    {
        float differenceX = endPos.x - startPos.x;
        float differenceY = endPos.y - startPos.y;

        if (Math.Abs(differenceX) >= Math.Abs(differenceY))
        {
            return differenceX > 0 ?
                MoveDirection.horizontalRight :
                MoveDirection.horizontalLeft;
        }
        else
        {
            return differenceY > 0 ?
                MoveDirection.vertivalTop :
                MoveDirection.verticalBottom;
        }
    }
}
