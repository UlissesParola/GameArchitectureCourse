using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    private bool _wasPickedUp;

    private void OnTriggerEnter(Collider other)
    {
        if (_wasPickedUp)
        {
            return;
        }

        var inventory = other.GetComponent<Inventory>();

        if (inventory != null)
        {
            inventory.PickUp(this);
            _wasPickedUp = true;
        }
    }
    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
