using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        Collider collider = GetComponent<Collider>();

        if (collider.isTrigger == false)
        {
            collider.isTrigger = true;
        }
    }
}

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Item item = (Item) target;

        EditorGUILayout.BeginHorizontal();
            if (item.Icon != null)
            {
                EditorGUILayout.LabelField("Item", GUILayout.Width(145));
                GUILayout.Box(item.Icon.texture, GUILayout.Height(40), GUILayout.Width(40));    
            }
            else
            {
                EditorGUILayout.HelpBox("No Icon Selected", MessageType.Warning);
            }

            EditorGUILayout.ObjectField(item.Icon, typeof(Sprite), false, GUILayout.Width(200));    
            
        EditorGUILayout.EndHorizontal();
        

        base.OnInspectorGUI();
    }
}
