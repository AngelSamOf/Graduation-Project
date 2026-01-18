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

    public static void ResetInstance()
    {
        _instance = null;
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
        Wins wins = _storage.FieldData.Wins;
        if (wins.IsSymbolWin)
        {
            victoryConditions += wins.SymbolConditions.Count;
        }
        if (wins.IsCombinationWin)
        {
            victoryConditions += wins.CombinationConditions.Count;
        }

        // Подписка на события если есть победные комбинации
        // И обнуление полей в хранилище
        if (wins.IsStepWin)
        {
            _storage.ResetStepCount();
            EventEmitter.EndMoveSymbol += CheckStep;
        }
        if (wins.IsSymbolWin)
        {
            foreach (FieldSymbol symbol in _storage.FieldData.Symbols)
            {
                _storage.AddSymbolCounter(symbol.Symbol.ID);
            }
            EventEmitter.WinCombination += CheckSymbol;
        }
        if (wins.IsCombinationWin)
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
        if (wins.IsEnemyKill)
        {
            EventEmitter.CharacterDeath += CheckKills;
        }

        Debug.Log("Contract \"Init Win Condition\": end Implement");
    }

    private void CheckStep()
    {
        _storage.IncreaseStep();
        if (_storage.StepCount >= _storage.FieldData.Wins.StepLimit)
        {
            _contractLoseGame.Implement();
        }
    }

    private void CheckSymbol(WinCombination data)
    {
        // Костыль
        // При кросс комбинации убираем 1 символ для правильного подсчёта
        int count = data.WinType == WinType.Destroy
            ? data.Positions.Count - 1
            : data.Positions.Count;

        _storage.IncreaseSymbolCounter(data.ID, count);
        SymbolCondition targetCondition = _storage.FieldData.Wins.SymbolConditions
            .Find(condition => condition.Symbol.ID == data.ID);

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
        CombinationCondition targetCondition = _storage.FieldData.Wins.CombinationConditions
            .Find(condition => condition.CombinationType == data.WinType);

        // Сохраняем, если есть такая комбинация
        if (targetCondition != null)
        {
            CheckWin(targetCondition, _storage.CombinationCount[data.WinType]);
        }
    }

    private void CheckWin(Condition condition, int currentValue)
    {
        if (condition.Count > currentValue || condition.IsComplete)
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

    private void CheckKills(bool isPlayer)
    {
        if (isPlayer)
        {
            _storage.IncreaseDeathPlayerCharacter();
            if (_storage.FieldData.PlayerCharacter.Count <= _storage.DeathPlayerCharacter)
            {
                _contractLoseGame.Implement();
            }
        }
        else
        {
            _storage.IncreaseDeathEnemyCharacter();
            if (_storage.FieldData.EnemyCharacter.Count <= _storage.DeathEnemyCharacter)
            {
                _contractWinGame.Implement();
            }
        }
    }
}