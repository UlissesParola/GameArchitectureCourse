using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    private Inventory _inventory;
    private Slot[] _slots;


    private void OnEnable()
    {
        _inventory = FindObjectOfType<Inventory>();
        _inventory.OnItemPickedUp += HandleItemPickedUp;
        _slots = GetComponentsInChildren<Slot>();
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