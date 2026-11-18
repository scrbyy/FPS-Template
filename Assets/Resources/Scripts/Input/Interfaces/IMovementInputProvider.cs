using System;
using UnityEngine;

namespace scrbyy.FpsTemplate.Input
{
    public interface IMovementInputProvider
    {
        public Vector2 MoveInput { get; }

        public abstract event Action OnJumpStarted;

        public abstract event Action OnSprintStarted;
        public abstract event Action OnSprintReleased;

        public abstract event Action OnDashStarted;
    }
}