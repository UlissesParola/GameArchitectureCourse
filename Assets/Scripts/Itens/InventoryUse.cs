using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class InventoryUse : MonoBehaviour
{
    private Inventory _inventory;

    // Start is called before the first frame update
    void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inventory.EquipedItem == null)
        {
            return;
        }

        foreach (var useAction in  _inventory.EquipedItem.Actions)
        {
            if (useAction.TargetComponent.CanUse && WasPressed(useAction.UseMode))
            {
                useAction.TargetComponent.Use();
            }
        }
        
    }

    private bool WasPressed(UseMode useMode)
    {
        switch(useMode)
        {
            case UseMode.LeftClick: return Input.GetMouseButtonDown(0);
            case UseMode.RightClick: return Input.GetMouseButtonDown(1);
            default: return false;
                    
        }
    }
}
