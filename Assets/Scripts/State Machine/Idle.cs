using UnityEngine;

public class Idle : IState
{
    public void Tick()
    {
        Debug.Log("On idle state");
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }
}