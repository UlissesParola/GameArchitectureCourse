using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Inventory: MonoBehaviour
{
    public event Action<Item> OnItemPickedUp;
    public event Action<Item> OnActiveItemChanged;

    private List<Item> _items;
    private Transform _itemsContainer;
    [SerializeField] private Transform _rightHand;
    

    public Item EquipedItem { get; private set; }

    private void Awake()
    {
        _itemsContainer = new GameObject("Items").transform;
        _itemsContainer.SetParent(transform);

        _items = new List<Item>();
    }

    public void PickUp(Item item)
    {
        _items.Add(item);
        OnItemPickedUp?.Invoke(item);
        item.transform.SetParent(_itemsContainer);
        Equip(item);
    }

    public void Equip(Item item)
    {
        Debug.Log($"Equipped item: {item.gameObject.name}");
        EquipedItem = item;
        EquipedItem.transform.SetParent(_rightHand);
        EquipedItem.transform.localPosition = Vector3.zero;
        EquipedItem.transform.localRotation = Quaternion.identity;

        OnActiveItemChanged?.Invoke(item);
        /*
        É o mesmo que: 
        if (ActiveItemChanged != null)
        {
            ActiveItemChanged(EquipedItem);
        }
        */

    }
}