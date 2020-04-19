using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Entity))]
public class EntityStateMachine : MonoBehaviour
{
    private StateMachine _stateMachine;

    private void Awake()
    {
        var idle = new Idle();
        var attack = new Attack();
        var chasePlayer = new ChasePlayer(GetComponent<NavMeshAgent>());
        
        _stateMachine = new StateMachine();
        _stateMachine.AddState(idle);
        _stateMachine.AddState(attack);
        _stateMachine.AddState(chasePlayer);

        _stateMachine.SetState(idle);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}