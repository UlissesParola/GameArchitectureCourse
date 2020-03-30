using System;
using NSubstitute.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    [SerializeField]private Image _iconImage;
    private TMP_Text _text;
    public bool IsEmpty => Item == null;
    public Item Item { get; private set; }
    public Image IconImage => _iconImage;

    private void Awake()
    {
        //_icon = GetComponentInChildren<Image>();
    }

    public void SetItem(Item item)
    {
        Item = item;
        _iconImage.sprite = item.Icon;
    }

    private void OnValidate()
    {
        _text = GetComponentInChildren<TMP_Text>();
        int hotkeyNumber = transform.GetSiblingIndex() + 1;
        _text.SetText(hotkeyNumber.ToString());
        gameObject.name = "Slot " + hotkeyNumber;
    }
}