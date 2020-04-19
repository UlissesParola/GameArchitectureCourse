using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StateMachine
{
    private List<IState> _stateList = new List<IState>();
    private IState _currentState;

    public void AddState(IState state)
    {
        _stateList.Add(state);
    }

    public void SetState(IState state)
    {
        if (_currentState == state)
        {
            return;
        }
        
        Debug.Log($"State changed to {state}");
        _currentState?.OnExit();
        _currentState = state;
        _currentState.OnEnter();
    }

    public void Tick()
    {
        _currentState.Tick();
    }
}