using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    private Inventory _inventory;
    private Slot[] _slots;
    private Player _player;


    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        _inventory = FindObjectOfType<Inventory>();
        _slots = GetComponentsInChildren<Slot>();
        
        _player.PlayerInput.OnHotkeyPressed += HandleHotkeyPressed;
        _inventory.OnItemPickedUp += HandleItemPickedUp;
    }

    private void OnDisable()
    {
        _player.PlayerInput.OnHotkeyPressed -= HandleHotkeyPressed;
        _inventory.OnItemPickedUp -= HandleItemPickedUp;
    }

    private void HandleHotkeyPressed(int index)
    {
        if (index < 0 || index >= _slots.Length )
        {
            return;
        }
        
        if (_slots[index].IsEmpty == false)
        {
            _inventory.Equip(_slots[index].Item);
        }
    }

    private void HandleItemPickedUp(Item item)
    {
        Slot slot = FindNextOpenSlot();
        if (slot != null)
        {
            slot.SetItem(item);
        }
    }

    private Slot FindNextOpenSlot()
    {
        
        foreach (Slot slot in _slots)
        {
            if (slot.IsEmpty)
            {
                return slot;
            }
        }
        
        Debug.LogError("No empty slots available");
        return null;
    }
}