using System;
using System.Collections.Generic;
using UnityEngine;

public class ContractInitWinCondition
{
    private static ContractInitWinCondition _instance;
    private ContractInitWinCondition() { }
    private BattleStorage _storage;
    private ContractLoseGame _contractLoseGame;
    private ContractWinGame _contractWinGame;

    public static ContractInitWinCondition GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Init Win Condition\": start Implement");
        // Инициализация данных
        _storage = BattleStorage.GetInstance();
        _contractLoseGame = ContractLoseGame.GetInstance();
        _contractWinGame = ContractWinGame.GetInstance();

        // Подсчёт победных условий
        int victoryConditions = 0;
        if (_storage.FieldData.IsSymbolWin)
        {
            victoryConditions += _storage.FieldData.SymbolConditions.Count;
        }
        if (_storage.FieldData.IsCombinationWin)
        {
            victoryConditions += _storage.FieldData.CombinationConditions.Count;
        }

        // Подписка на события если есть победные комбинации
        // И обнуление полей в хранилище
        if (_storage.FieldData.IsStepWin)
        {
            _storage.ResetStepCount();
            EventEmitter.EndMoveSymbol += CheckStep;
        }
        if (_storage.FieldData.IsSymbolWin)
        {
            foreach (FieldSymbol symbol in _storage.FieldData.Symbols)
            {
                _storage.AddSymbolCounter(symbol.Symbol.ID);
            }
            EventEmitter.WinCombination += CheckSymbol;
        }
        if (_storage.FieldData.IsCombinationWin)
        {
            foreach (WinType type in Enum.GetValues(typeof(WinType)))
            {
                if (type == WinType.NotSelected || type == WinType.NotSelected)
                {
                    continue;
                }
                _storage.AddWinCounter(type);
            }
            EventEmitter.WinCombination += CheckCombination;
        }

        Debug.Log("Contract \"Init Win Condition\": end Implement");
    }

    private void CheckStep()
    {
        _storage.IncreaseStep();
        Debug.Log($"Step: {_storage.StepCount}:{_storage.FieldData.StepLimit}");
        if (_storage.StepCount >= _storage.FieldData.StepLimit)
        {
            _contractLoseGame.Implement();
        }
    }

    private void CheckSymbol(WinCombination data)
    {
        _storage.IncreaseSymbolCounter(data.ID, data.Positions.Count);
        SymbolCondition targetCondition = _storage.FieldData.SymbolConditions
            .Find(condition => condition.SymbolID == data.ID);

        // Сохраняем, если есть такое условие победы
        if (targetCondition != null)
        {
            CheckWin(targetCondition, _storage.SymbolCount[data.ID]);
        }
    }

    private void CheckCombination(WinCombination data)
    {
        // Скип кодовых комбинаций
        if (data.WinType == WinType.Destroy || data.WinType == WinType.NotSelected)
        {
            return;
        }

        _storage.IncreaseWinCounter(data.WinType);
        CombinationCondition targetCondition = _storage.FieldData.CombinationConditions
            .Find(condition => condition.CombinationType == data.WinType);

        // Сохраняем, если есть такая комбинация
        if (targetCondition != null)
        {
            CheckWin(targetCondition, _storage.CombinationCount[data.WinType]);
        }
    }

    private void CheckWin(Condition condition, int currentValue)
    {
        Debug.Log($"{currentValue}:{condition.Count}");
        if (condition.Count <= currentValue)
        {
            if (condition.IsComplete)
            {
                return;
            }
            condition.Complete();
            _storage.IncreaseVictoryCondition();
            if (_storage.VictoryConditions <= _storage.FulfilledVictoryConditions)
            {
                _contractWinGame.Implement();
            }
        }
    }
}