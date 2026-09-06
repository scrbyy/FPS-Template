using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameFSM : MonoBehaviour
{
    private Dictionary<Type, IState> _states;
    public IState CurrentState { get; private set; }

    [Inject]
    private void Construct(List<IState> states)
    {
        _states = new Dictionary<Type, IState>();

        foreach (var state in states)
        {
            _states[state.GetType()] = state;
        }
    }

    private void Start()
    {
        SetState<PlayState>();
    }

    public void SetState<TState>() where TState : IState
    {
        var type = typeof(TState);

        if (!_states.TryGetValue(type, out var newState))
        {
            return;
        }

        if (CurrentState == newState) return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}