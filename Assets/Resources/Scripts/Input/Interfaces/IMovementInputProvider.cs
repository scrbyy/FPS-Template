using System;
using UnityEngine;

namespace scrbyy.FpsTemplate.InputSystem.Interfaces
{
    public interface IMovementInputProvider
    {
        public Vector2 MoveInput { get; }

        public event Action OnJumpStarted;

        public event Action OnSprintStarted;
        public event Action OnSprintReleased;

        public event Action OnDashStarted;
    }
}