using System;
using System.Collections.Generic;
using UnityEngine;


public class Inventory: MonoBehaviour
{
    private List<Item> _items;
    private Transform _itemsContainer;

    private void Awake()
    {
        _itemsContainer = new GameObject("Items").transform;
        _itemsContainer.SetParent(transform);

        _items = new List<Item>();
    }

    public void PickUp(Item item)
    {
        _items.Add(item);
        item.transform.SetParent(_itemsContainer);
        Equip(item);
    }

    private void Equip(Item item)
    {
        Debug.LogError($"Equipped item: {item.gameObject.name}");
    }
}