using System.Collections.Generic;
using UnityEngine;

public class ContractInitCharacter
{
    private static ContractInitCharacter _instance;
    private ContractInitCharacter() { }
    private BattleStorage _storage;

    public static ContractInitCharacter GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Init Character\": start Implement");
        // Инициализация данных
        _storage = BattleStorage.GetInstance();

        // Добавление на поле основных контейнеров
        (GameObject characterContainer, GameObject enemyContainer) = GenerateCharacterContainer();

        // Генерация персонажей
        List<CharacterBase> characters = GenerateCharacter(characterContainer);

        Debug.Log("Contract \"Init Character\": end Implement");
    }

    private (GameObject characterContainer, GameObject enemyContainer) GenerateCharacterContainer()
    {
        GameObject playerCharacterContainer = new("player-character-container");
        GameObject enemyCharacterContainer = new("enemy-character-container");

        Field field = _storage.FieldData.Field;
        playerCharacterContainer.transform.position = new Vector3(
            (field.SizeX / 2 + _storage.Constants.CharacterShift) * field.StepX * -1,
            0
        );
        enemyCharacterContainer.transform.position = new Vector3(
            (field.SizeX / 2 + _storage.Constants.CharacterShift) * field.StepX,
            0
        );

        return (playerCharacterContainer, enemyCharacterContainer);
    }

    private List<CharacterBase> GenerateCharacter(GameObject container)
    {
        List<CharacterBase> charactersList = new();

        foreach (CharacterObject characterData in _storage.FieldData.PlayerCharacter)
        {
            GameObject character = new(characterData.name);
            character.transform.SetParent(container.transform);
            character.transform.localPosition = Vector3.zero;
            CharacterBase characterBase = character.AddComponent<CharacterBase>();
            characterBase.Init(characterData);
            charactersList.Add(characterBase);
        }

        return charactersList;
    }
}