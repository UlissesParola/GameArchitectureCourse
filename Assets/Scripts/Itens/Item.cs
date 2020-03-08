using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    private bool _wasPickedUp;

    [SerializeField] private CrosshairDefinition _crosshairDefinition;
    [SerializeField] private UseAction[] _actions;
    [SerializeField] private Sprite _icon;
    public UseAction[] Actions => _actions;
    public CrosshairDefinition CrosshairDefinition => _crosshairDefinition;
    public Sprite Icon => _icon;

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
