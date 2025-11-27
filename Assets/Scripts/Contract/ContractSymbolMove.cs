using System.Collections.Generic;
using UnityEngine;

public class ContractSymbolMove
{
    private static ContractSymbolMove _instance;
    private ContractSymbolMove() { }
    private BattleStorage storage;

    public static ContractSymbolMove GetInstance()
    {
        if (_instance == null)
        {
            _instance = new();
        }
        return _instance;
    }

    public void Implement(
        Direction direction,
        SymbolBase symbol
    )
    {
        Debug.Log("Contract \"SymbolMove\": start Implement");
        // Инициализация данных
        storage = BattleStorage.GetInstance();

        // Получение текущего символа на поле
        SymbolBase currentSymbol = storage.SymbolMap[symbol.Position.X, symbol.Position.Y];
        CellBase currentCell = storage.FieldMap[symbol.Position.X, symbol.Position.Y];

        // Получение новой позиции на поле
        CellPosition targetPosition = GetTargetPosition(direction, symbol.Position);

        if (
            targetPosition.X < 0 ||
            targetPosition.Y < 0 ||
            targetPosition.X >= storage.FieldData.SizeX ||
            targetPosition.Y >= storage.FieldData.SizeY
        )
        {
            currentSymbol.MoveSymbolAndReturn(
                storage.Constants.ShiftTime,
                currentCell.transform.localPosition + GetOutDirection(direction)
            );
            return;
        }

        // Получение итогового символа на поле
        SymbolBase targetSymbol = storage.SymbolMap[targetPosition.X, targetPosition.Y];
        CellBase targetCell = storage.FieldMap[targetPosition.X, targetPosition.Y];

        // Смена позиций символов
        SymbolMethods.SwapPosition(storage, currentSymbol, targetSymbol);

        // Запуск анимаций смены
        currentSymbol.MoveSymbol(
            storage.Constants.MoveTime,
            targetCell.transform.localPosition
        );
        targetSymbol.MoveSymbol(
            storage.Constants.MoveTime,
            currentCell.transform.localPosition
        );

        Debug.Log("Contract \"SymbolMove\": end Implement");
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
            case Direction.vertivalTop:
                return new(position.X, position.Y - 1);
            case Direction.verticalBottom:
                return new(position.X, position.Y + 1);
            default:
                return null;
        }
    }

    private Vector3 GetOutDirection(
        Direction direction
    )
    {
        switch (direction)
        {
            case Direction.horizontalRight:
                return new(storage.Constants.ShiftMove, 0);
            case Direction.horizontalLeft:
                return new(-storage.Constants.ShiftMove, 0);
            case Direction.vertivalTop:
                return new(0, storage.Constants.ShiftMove);
            case Direction.verticalBottom:
                return new(0, -storage.Constants.ShiftMove);
            default:
                return Vector3.zero;
        }
    }
}