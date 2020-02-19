using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    private bool _wasPickedUp;

    [SerializeField] private CrosshairDefinition _crosshairDefinition;
    [SerializeField] private UseAction[] _actions;
    public UseAction[] Actions => _actions;
    public CrosshairDefinition CrosshairDefinition => _crosshairDefinition;

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
