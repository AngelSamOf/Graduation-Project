using Unity.VisualScripting;
using UnityEngine;

public class ContractShowSpell
{
    private static ContractShowSpell _instance;
    private ContractShowSpell() { }

    private BattleStorage _storage;
    private Transform _spellContainer = null;

    public static ContractShowSpell GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public static void ResetInstance()
    {
        _instance = null;
    }

    public void Implement(PlayerCharacterComponent character)
    {
        Debug.Log("Contract \"Show Spell\": start Implement");

        // Инициализация данных
        _storage = BattleStorage.GetInstance();

        if (_spellContainer == null)
        {
            _spellContainer = GenerateSpellContainer();
        }
        else
        {
            foreach (Transform child in _spellContainer.transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        GenerateSpell(character);
        GeneratePassiveSpell(character);

        Debug.Log("Contract \"Show Spell\": end Implement");
    }

    private Transform GenerateSpellContainer()
    {
        GameObject spellContainer = new("spell-container");
        spellContainer.transform.position = new(0, -6.7f, -2f);
        return spellContainer.transform;
    }

    private void GenerateSpell(PlayerCharacterComponent character)
    {
        int index = 0;
        foreach (BaseSpell spellData in character.Data.Spells)
        {
            GameObject spell = new($"spell-{index}");
            spell.transform.SetParent(_spellContainer);
            spell.transform.localPosition = new(index * _storage.FieldData.Constants.IconShift, 0f);
            SpellComponent spellComponent = spell.AddComponent<SpellComponent>();
            spellComponent.Init(spellData, character);

            index += 1;
        }
    }

    private void GeneratePassiveSpell(PlayerCharacterComponent character)
    {
        GameObject spell = new("passive-spell");
        spell.transform.SetParent(_spellContainer);
        spell.transform.localPosition = new(3.4f, 0f);
        SpriteRenderer spellRenderer = spell.AddComponent<SpriteRenderer>();
        spellRenderer.sprite = character.Data.PassiveSpell.Texture;
    }
}