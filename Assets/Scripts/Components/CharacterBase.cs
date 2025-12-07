using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterBase : MonoBehaviour, IPointerClickHandler
{
    protected CharacterObject _data;
    protected BattleStorage _storage;

    protected BoxCollider2D _collider;
    protected SpriteRenderer _mainTexture;

    protected List<EnergyComponent> _energyList;
    protected List<HPComponent> _hpList;

    public int CurrentHP => _currentHP;
    protected int _currentHP;

    public int CurrentEnergy => _currentEnergy;
    protected int _currentEnergy;

    public void Init(CharacterObject data)
    {
        // Сохранение данных
        _data = data;
        _storage = BattleStorage.GetInstance();

        _currentEnergy = 0;
        _currentHP = _data.HP;

        // Получение компонентов  с объекта
        _collider = GetComponent<BoxCollider2D>();
        _collider.size = _storage.Constants.CharacterColliderSize;

        // Инициализация дополнительных компонентов
        InitSubComponents();

        // Подписка на события
        EventEmitter.WinCombination += UpdateEnergy;
    }

    public void UpdateEnergy(WinCombination win)
    {
        // Проверка на символ
        if (win.ID != _data.Symbol.ID)
        {
            return;
        }

        // Проверка что энергии не максимум
        if (_currentEnergy >= _data.Energy)
        {
            return;
        }

        _energyList[_currentEnergy].SetState(true);
        _currentEnergy += 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(_data.name);
    }

    private void InitSubComponents()
    {
        // Создание текстуры персонажа
        GameObject characterTextureObj = new("texture");
        characterTextureObj.transform.SetParent(transform);
        characterTextureObj.transform.localPosition = Vector3.zero;
        SpriteRenderer characterTexture = characterTextureObj.AddComponent<SpriteRenderer>();
        characterTexture.sprite = _data.Texure;
        _mainTexture = characterTexture;

        // Создание дополнительных контейнеров
        GameObject energyContainer = new("energy-container");
        energyContainer.transform.SetParent(transform);
        energyContainer.transform.localPosition = new(
            _storage.Constants.CharacterSubContainerShift,
            _storage.Constants.CharacterColliderSize.y / 2
        );
        _energyList = GenerateEnergy(energyContainer.transform);

        GameObject hpContainer = new("hp-container");
        hpContainer.transform.SetParent(transform);
        hpContainer.transform.localPosition = new(
            -_storage.Constants.CharacterSubContainerShift,
            _storage.Constants.CharacterColliderSize.y / 2
        );
        _hpList = GenerateHP(hpContainer.transform);
    }

    private List<EnergyComponent> GenerateEnergy(Transform container)
    {
        List<EnergyComponent> energyComponents = new();
        // Создание иконки энергии
        GameObject symbol = new($"energy-symbol");
        symbol.transform.SetParent(container);
        symbol.transform.localScale = new(0.5f, 0.5f);
        symbol.transform.localPosition = new(0f, _storage.Constants.IconShift);
        SpriteRenderer symbolSprite = symbol.AddComponent<SpriteRenderer>();
        symbolSprite.sprite = _data.Symbol.Texture;

        // Создание энергии
        for (int i = 0; i < _data.Energy; i++)
        {
            GameObject energy = new($"energy-{i}");
            energy.transform.SetParent(container);
            energy.transform.localPosition = new(0f, _storage.Constants.IconShift * i * -1);
            EnergyComponent energyComponent = energy.AddComponent<EnergyComponent>();
            energyComponent.Init(_data.EnergyData);
            energyComponents.Add(energyComponent);
        }

        return energyComponents;
    }
    private List<HPComponent> GenerateHP(Transform container)
    {
        List<HPComponent> hpComponents = new();
        // Создание энергии
        for (int i = 0; i < _data.Energy; i++)
        {
            GameObject hp = new($"hp-{i}");
            hp.transform.SetParent(container);
            hp.transform.localPosition = new(0f, _storage.Constants.IconShift * i * -1);
            HPComponent hpComponent = hp.AddComponent<HPComponent>();
            hpComponent.Init(_data.HPData);
            hpComponents.Add(hpComponent);
        }

        return hpComponents;
    }
}
