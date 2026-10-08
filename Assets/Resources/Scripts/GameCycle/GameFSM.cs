using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameFSM : MonoBehaviour
{
    private Dictionary<Type, IGameState> _states;
    public IGameState CurrentState { get; private set; }

    [Inject]
    private void Construct(List<IGameState> states)
    {
        _states = new Dictionary<Type, IGameState>();

        foreach (var state in states)
        {
            _states[state.GetType()] = state;
        }
    }

    private void Start()
    {
        SetState<GameplayState>();
    }

    public void SetState<TState>() where TState : IGameState
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