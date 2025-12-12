using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContractInitTexture
{
    private static ContractInitTexture _instance;
    private ContractInitTexture() { }
    private BattleStorage _storage;
    private RectTransform _canvasTransform;
    private RectTransform _conditionContainer;

    private TextMeshPro _tmpStep;
    private Dictionary<string, TextMeshPro> _tmpSymbols = new();

    public static ContractInitTexture GetInstance()
    {
        _instance ??= new();
        return _instance;
    }

    public void Implement()
    {
        Debug.Log("Contract \"Init Texture\": start Implement");
        // Инициализация данных
        _storage = BattleStorage.GetInstance();

        InitBackground();
        InitUI();

        Debug.Log("Contract \"Init Texture\": end Implement");
    }

    private void InitBackground()
    {
        // Задний фон
        GameObject background = new("bakcground");
        background.transform.position = new Vector3(0, 0, 2);
        SpriteRenderer bgSpriteRenderer = background.AddComponent<SpriteRenderer>();
        bgSpriteRenderer.sprite = _storage.FieldData.Textures.Background;

        // Верхняя панель
        GameObject topPanel = new("top-panel");
        topPanel.transform.position = new Vector3(0, _storage.FieldData.Constants.TopPanelY, -1);
        SpriteRenderer topPanelRenderer = topPanel.AddComponent<SpriteRenderer>();
        topPanelRenderer.sprite = _storage.FieldData.Textures.TopPanel;

        // Нижняя панель
        GameObject bottomPanel = new("bottom-panel");
        bottomPanel.transform.position = new Vector3(0, -_storage.FieldData.Constants.TopPanelY, -1);
        SpriteRenderer bottomPanelRenderer = bottomPanel.AddComponent<SpriteRenderer>();
        bottomPanelRenderer.sprite = _storage.FieldData.Textures.BottomPanel;
    }

    private void InitUI()
    {
        // Получение Canvas
        GameObject canvas = GameObject.Find("canvas");
        if (canvas == null)
        {
            throw new Exception("Not found Canvas in game scene!");
        }
        _canvasTransform = canvas.GetComponent<RectTransform>();
        Constants constants = _storage.FieldData.Constants;

        // Контейнер для победного UI
        GameObject condition = new("condition-container");
        GridLayoutGroup confitionLayout = condition.AddComponent<GridLayoutGroup>();
        _conditionContainer = condition.GetComponent<RectTransform>();
        _conditionContainer.SetParent(_canvasTransform);
        _conditionContainer.localPosition = new(
            constants.ConditionPoxX * constants.CanvasPPI,
            constants.ConditionPoxY * constants.CanvasPPI,
            0
        );
        _conditionContainer.localScale = new(1, 1, 1);
        confitionLayout.cellSize = new(1, 1);
        confitionLayout.spacing = new(constants.ConditionSpacing, 0);
        confitionLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        confitionLayout.constraintCount = 1;

        InitStep();
        InitSymbolConditions();
    }

    private void InitStep()
    {
        Wins wins = _storage.FieldData.Wins;
        if (!wins.IsStepWin) return;

        GameObject tmpObject = new("step-text");
        _tmpStep = tmpObject.AddComponent<TextMeshPro>();
        RectTransform tmpTransform = _tmpStep.GetComponent<RectTransform>();
        tmpTransform.SetParent(_conditionContainer);
        tmpTransform.localPosition = new(0, 0, 0);
        _tmpStep.fontSize = 8;
        _tmpStep.text = $"0/{wins.StepLimit}";
        _tmpStep.color = new(0, 0, 0);
        _tmpStep.enableWordWrapping = false;

        GameObject icon = new("step-icon");
        Image iconImage = icon.AddComponent<Image>();
        RectTransform iconTransform = icon.GetComponent<RectTransform>();
        iconTransform.SetParent(_conditionContainer);
        iconTransform.localPosition = new(0, 0, 0);
        iconImage.sprite = _storage.FieldData.Textures.IconStep;

        EventEmitter.EndMoveSymbol += UpdateStep;
    }

    public void UpdateStep()
    {
        _tmpStep.text = $"{_storage.StepCount}/{_storage.FieldData.Wins.StepLimit}";
    }

    private void InitSymbolConditions()
    {
        Wins wins = _storage.FieldData.Wins;
        if (!wins.IsSymbolWin) return;

        foreach (SymbolCondition condition in wins.SymbolConditions)
        {
            GameObject tmpObject = new($"{condition.Symbol.ID}-text");
            TextMeshPro tmp = tmpObject.AddComponent<TextMeshPro>();
            RectTransform tmpTransform = tmp.GetComponent<RectTransform>();
            tmpTransform.SetParent(_conditionContainer);
            tmpTransform.localPosition = new(0, 0, 0);
            tmp.fontSize = 8;
            tmp.text = $"0/{condition.Count}";
            tmp.color = new(0, 0, 0);
            tmp.enableWordWrapping = false;
            _tmpSymbols.Add(condition.Symbol.ID, tmp);

            GameObject icon = new($"{condition.Symbol.ID}-icon");
            Image iconImage = icon.AddComponent<Image>();
            RectTransform iconTransform = icon.GetComponent<RectTransform>();
            iconTransform.SetParent(_conditionContainer);
            iconTransform.localPosition = new(0, 0, 0);
            iconImage.sprite = condition.Symbol.Icon;
        }

        EventEmitter.WinCombination += UpdateSymbol;
    }

    public void UpdateSymbol(WinCombination combination)
    {
        if (!_tmpSymbols.ContainsKey(combination.ID))
        {
            return;
        }
        TextMeshPro tmp = _tmpSymbols[combination.ID];
        SymbolCondition targetCondition = _storage.FieldData.Wins.SymbolConditions
            .Find(condition => condition.Symbol.ID == combination.ID);

        tmp.text = $"{_storage.SymbolCount[combination.ID]}/{targetCondition.Count}";
    }
}