using System;

namespace scrbyy.FpsTemplate.Input
{
    public interface IInteractionInputProvider
    {
        public event Action OnInteractStarted;
    }
}