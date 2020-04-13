using UnityEngine;
public class ItemRaycaster : ItemComponent
{
    [SerializeField ]private float _delay = 0.1f;
    [SerializeField] private float _range = 10f;
    [SerializeField]private int _damage;

    private RaycastHit[] _results = new RaycastHit[100];
    private int _layerMask;

    private void Awake()
    {
        _layerMask = LayerMask.GetMask("Default");
    }

    public override void Use()
    {
        _timeNextUse = Time.time + _delay;

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one / 2);
        int hits = Physics.RaycastNonAlloc(ray, _results, _range, _layerMask, QueryTriggerInteraction.Collide);

        double nearestDistance = double.MaxValue;
        RaycastHit nearestHit = new RaycastHit();

        for (int i = 0; i < hits; i++)
        {
            double distance = _results[i].distance;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestHit = _results[i];
            }
        }

        if (hits > 0)
        {
            var nearestObject = nearestHit.transform.GetComponent<ITakeHits>();
            nearestObject?.TakeHit(_damage);
        }
        

    }
}
