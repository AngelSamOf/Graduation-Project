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
        MoveDirection direction,
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
            currentSymbol.gameObject.AddComponent<MoveAction>()
                .MoveWithTime(
                    storage.Constants.ShiftTime,
                    currentCell.NodeTransform.localPosition + GetOutDirection(direction),
                    () =>
                    {
                        currentSymbol.gameObject.AddComponent<MoveAction>()
                        .MoveWithTime(
                            storage.Constants.ShiftTime,
                            currentCell.NodeTransform.localPosition
                        );
                    }
                );
            return;
        }

        // Получение итогового символа на поле
        SymbolBase targetSymbol = storage.SymbolMap[targetPosition.X, targetPosition.Y];
        CellBase targetCell = storage.FieldMap[targetPosition.X, targetPosition.Y];

        // Установка новых позиций символов
        SymbolBase temp = storage.SymbolMap[targetPosition.X, targetPosition.Y];
        storage.SymbolMap[targetPosition.X, targetPosition.Y] =
            storage.SymbolMap[currentSymbol.Position.X, currentSymbol.Position.Y];
        storage.SymbolMap[currentSymbol.Position.X, currentSymbol.Position.Y] = temp;

        targetSymbol.SetPosition(currentSymbol.Position);
        currentSymbol.SetPosition(targetPosition);

        // Запуск анимаций смены
        currentSymbol.gameObject.AddComponent<MoveAction>()
            .MoveWithTime(
                storage.Constants.MoveTime,
                targetCell.NodeTransform.localPosition
            );
        targetSymbol.gameObject.AddComponent<MoveAction>()
            .MoveWithTime(
                storage.Constants.MoveTime,
                currentCell.NodeTransform.localPosition
            );

        Debug.Log("Contract \"SymbolMove\": end Implement");
    }

    private CellPosition GetTargetPosition(
        MoveDirection direction,
        CellPosition position
    )
    {
        switch (direction)
        {
            case MoveDirection.horizontalRight:
                return new(position.X + 1, position.Y);
            case MoveDirection.horizontalLeft:
                return new(position.X - 1, position.Y);
            case MoveDirection.vertivalTop:
                return new(position.X, position.Y - 1);
            case MoveDirection.verticalBottom:
                return new(position.X, position.Y + 1);
            default:
                return null;
        }
    }

    private Vector3 GetOutDirection(
        MoveDirection direction
    )
    {
        switch (direction)
        {
            case MoveDirection.horizontalRight:
                return new(storage.Constants.ShiftMove, 0);
            case MoveDirection.horizontalLeft:
                return new(-storage.Constants.ShiftMove, 0);
            case MoveDirection.vertivalTop:
                return new(0, storage.Constants.ShiftMove);
            case MoveDirection.verticalBottom:
                return new(0, -storage.Constants.ShiftMove);
            default:
                return Vector3.zero;
        }
    }
}

public enum MoveDirection
{
    horizontalLeft = 0,
    horizontalRight = 1,
    vertivalTop = 2,
    verticalBottom = 3
}