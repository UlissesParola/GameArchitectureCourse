using UnityEngine;

public abstract class ItemComponent : MonoBehaviour
{
    protected float _timeNextUse;
    public bool CanUse => Time.time >= _timeNextUse;
    public abstract void Use();
}
