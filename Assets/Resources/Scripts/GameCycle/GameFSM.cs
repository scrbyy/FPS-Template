using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace scrbyy.FpsTemplate.GameCycle
{
    public class GameFSM : MonoBehaviour
    {
        public event Action<IGameState> OnStateChanged;
        public IGameState CurrentState { get; private set; }

        private Dictionary<Type, IGameState> _states;

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
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}