using System;

namespace scrbyy.FpsTemplate.InputSystem.Interfaces
{
    public interface IUIMenuProvider
    {
        public event Action OnDebugMenuToggled;
        public event Action OnPauseMenuToggled;
    }
}
