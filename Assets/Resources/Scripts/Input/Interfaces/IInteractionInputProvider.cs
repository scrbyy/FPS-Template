using System;

namespace scrbyy.FpsTemplate.InputSystem
{
    public interface IInteractionInputProvider
    {
        public event Action OnInteractStarted;
    }
}