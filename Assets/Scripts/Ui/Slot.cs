using System;
using NSubstitute.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public bool IsEmpty => _item == null;
    private Item _item;
    private Image _icon;

    private void Awake()
    {
        _icon = GetComponentInChildren<Image>();
    }

    public void SetItem(Item item)
    {
        _item = item;
        _icon.sprite = item.Icon;
    }
}