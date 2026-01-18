using System;
using UnityEngine;

public class ContractCheckData
{
    private static ContractCheckData _instance;
    private ContractCheckData() { }
    private BattleStorage _storage;

    public static ContractCheckData GetInstance()
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
        Debug.Log("Contract \"Check Data\": start Implement");
        // Инициализация данных
        _storage = BattleStorage.GetInstance();

        // Запуск проверок данных
        CharacterCheck();
        SymbolWeightCheck();
        WinConditionCheck();


        Debug.Log("Contract \"Check Data\": end Implement");
    }

    private void CharacterCheck()
    {
        if (_storage.FieldData.PlayerCharacter.Count == 0)
        {
            throw new Exception("More than one player character has not been added");
        }
    }

    private void SymbolWeightCheck()
    {
        float weight = 0f;
        foreach (FieldSymbol symbol in _storage.FieldData.Symbols)
        {
            weight += symbol.Weight;
        }

        if (weight != 1)
        {
            throw new Exception("Symbol total weight is not equal to zero");
        }
    }

    private void WinConditionCheck()
    {
        Wins wins = _storage.FieldData.Wins;

        // Проверка что установлены победные условия
        if (
            !wins.IsStepWin
            && !wins.IsSymbolWin
            && !wins.IsCombinationWin
        )
        {
            throw new Exception("There are no winning conditions set for the level");
        }

        // Проверка что количетсво ходов больше 0
        if (wins.IsStepWin && wins.StepLimit <= 0)
        {
            throw new Exception("The number of moves must be greater than 0");
        }

        // Проверка символов
        if (wins.IsSymbolWin)
        {
            if (wins.SymbolConditions.Count <= 0)
            {
                throw new Exception("No character set conditions have been added");
            }
            foreach (SymbolCondition condition in wins.SymbolConditions)
            {
                if (condition.Count <= 0)
                {
                    throw new Exception($"The number that must be scored to win is not specified for the {condition.Symbol.ID} symbol");
                }
            }
        }

        // Проверка победных комбинаций
        if (wins.IsCombinationWin)
        {
            if (wins.CombinationConditions.Count <= 0)
            {
                throw new Exception("Not a single set of winning combinations is specified");
            }
            foreach (CombinationCondition condition in wins.CombinationConditions)
            {
                if (
                    condition.CombinationType == WinType.NotSelected
                    || condition.CombinationType == WinType.Destroy
                )
                {
                    throw new Exception("Invalid winning combination type is specified");
                }
                if (condition.Count <= 0)
                {
                    throw new Exception("No number is specified for the winning combination");
                }
            }
        }
    }
}