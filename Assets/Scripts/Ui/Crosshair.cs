using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Crosshair : MonoBehaviour
{
    [SerializeField] private Image _crosshairImage;
    [SerializeField] private CrosshairDefinition _invalidSprite;
    private Inventory _inventory;
    

    private void OnEnable()
    {
        _inventory = FindObjectOfType<Inventory>();
        _inventory.OnActiveItemChanged += HandleActiveItemChanged;

        if (_inventory.EquipedItem != null)
        {
            _crosshairImage.sprite = _invalidSprite.Sprite;
        }
        else
        {
            HandleActiveItemChanged(_inventory.EquipedItem);
        }
    }

    private void OnValidate()
    {
        _crosshairImage = GetComponent<Image>();
    }
    private void HandleActiveItemChanged(Item item)
    {
        if (item?.CrosshairDefinition != null)
        {
            _crosshairImage.sprite = item.CrosshairDefinition.Sprite;
            Debug.Log($"Crosshair detected {item.CrosshairDefinition.Sprite.name}");
        }
        else
        {
            _crosshairImage.sprite = _invalidSprite.Sprite;
        }       
    }
}
