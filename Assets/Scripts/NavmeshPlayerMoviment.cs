using UnityEngine;
using UnityEngine.AI;

public class NavmeshPlayerMoviment : IPlayerMoviment
{
    private Player _player;
    private NavMeshAgent _navMeshAgent;

    public NavmeshPlayerMoviment(Player player)
    {
        _player = player;
        _navMeshAgent = player.GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = true;
    }
    public void Tick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _navMeshAgent.SetDestination(hit.point);
            }
        }
 
    }
}
