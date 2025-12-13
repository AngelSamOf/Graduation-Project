using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class SpellComponent : MonoBehaviour, IPointerClickHandler
{
    private BaseSpell _data;
    PlayerCharacterComponent _character;

    public void Init(BaseSpell data, PlayerCharacterComponent character)
    {
        _data = data;
        _character = character;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = _data.Texture;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new(1.0f, 1.0f);

        GenerateEnergy();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Игнороируем если не достаточно энергии
        if (_character.CurrentEnergy < _data.Cost)
        {
            return;
        }

        _data.Implement();
        _character.RemoveEnergy(_data.Cost);
    }

    private void GenerateEnergy()
    {
        for (int i = 0; i < _data.Cost; i++)
        {
            GameObject energy = new($"energy-{i}");
            energy.transform.SetParent(transform);
            energy.transform.localScale = new(0.25f, 0.25f);
            energy.transform.localPosition = new(-0.3f + (0.25f * i), -0.6f);
            SpriteRenderer energyRenderer = energy.AddComponent<SpriteRenderer>();
            energyRenderer.sprite = _character.Data.EnergyData.TextureFill;
        }
    }
}
