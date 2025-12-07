using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ContractSymbolMove
{
    private static ContractSymbolMove _instance;
    private ContractSymbolMove() { }
    private BattleStorage _storage;
    private bool _isImplement = false;

    public static ContractSymbolMove GetInstance()
    {
        if (_instance == null)
        {
            _instance = new();
        }
        return _instance;
    }

    public async Task Implement(
        Direction direction,
        SymbolBase symbol
    )
    {
        if (_isImplement)
        {
            Debug.Log("Contract \"Symbol Move\": aborted. There is already a running contract");
            return;
        }

        Debug.Log("Contract \"Symbol Move\": start Implement");
        _isImplement = true;
        // Инициализация данных
        _storage = BattleStorage.GetInstance();
        List<Task> tasks = new();

        // Получение текущего символа на поле
        SymbolBase currentSymbol = _storage.SymbolMap[symbol.Position.X, symbol.Position.Y];
        CellBase currentCell = _storage.FieldMap[symbol.Position.X, symbol.Position.Y];

        // Получение новой позиции на поле
        CellPosition targetPosition = GetTargetPosition(direction, symbol.Position);

        if (
            targetPosition.X < 0 ||
            targetPosition.Y < 0 ||
            targetPosition.X >= _storage.FieldData.Field.SizeX ||
            targetPosition.Y >= _storage.FieldData.Field.SizeY
        )
        {
            tasks.Add(currentSymbol.MoveSymbolAndReturn(
                _storage.Constants.ShiftTime,
                currentCell.transform.localPosition + GetOutDirection(direction)
            ));

            await Task.WhenAll(tasks);
            Debug.Log("Contract \"Symbol Move\": end Implement");
            _isImplement = false;
            return;
        }

        // Получение итогового символа на поле
        SymbolBase targetSymbol = _storage.SymbolMap[targetPosition.X, targetPosition.Y];
        CellBase targetCell = _storage.FieldMap[targetPosition.X, targetPosition.Y];

        // Смена позиций символов
        SymbolMethods.SwapPosition(_storage, currentSymbol, targetSymbol);

        // Запуск анимаций смены
        tasks.Add(currentSymbol.MoveSymbol(
            _storage.Constants.MoveTime,
            targetCell.transform.localPosition
        ));
        tasks.Add(targetSymbol.MoveSymbol(
            _storage.Constants.MoveTime,
            currentCell.transform.localPosition
        ));

        await Task.WhenAll(tasks);
        Debug.Log("Contract \"Symbol Move\": end Implement");
        _isImplement = false;
        // Вызов события на окончания передвижения
        EventEmitter.EndMoveSymbol.Invoke();
    }

    private CellPosition GetTargetPosition(
        Direction direction,
        CellPosition position
    )
    {
        switch (direction)
        {
            case Direction.horizontalRight:
                return new(position.X + 1, position.Y);
            case Direction.horizontalLeft:
                return new(position.X - 1, position.Y);
            case Direction.verticalTop:
                return new(position.X, position.Y - 1);
            case Direction.verticalBottom:
                return new(position.X, position.Y + 1);
            default:
                throw new Exception("The direction is not defined");
        }
    }

    private Vector3 GetOutDirection(
        Direction direction
    )
    {
        switch (direction)
        {
            case Direction.horizontalRight:
                return new(_storage.Constants.ShiftMove, 0);
            case Direction.horizontalLeft:
                return new(-_storage.Constants.ShiftMove, 0);
            case Direction.verticalTop:
                return new(0, _storage.Constants.ShiftMove);
            case Direction.verticalBottom:
                return new(0, -_storage.Constants.ShiftMove);
            default:
                return Vector3.zero;
        }
    }
}