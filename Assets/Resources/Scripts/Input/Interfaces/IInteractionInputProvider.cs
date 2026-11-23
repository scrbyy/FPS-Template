using System;

namespace scrbyy.FpsTemplate.InputSystem.Interfaces
{
    public interface IInteractionInputProvider
    {
        public event Action OnInteractStarted;
    }
}