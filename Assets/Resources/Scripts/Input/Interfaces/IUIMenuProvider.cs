using System;

namespace scrbyy.FpsTemplate.Input.Interfaces
{
    public interface IUIMenuProvider
    {
        public event Action OnDebugMenuToggled;
        public event Action OnPauseMenuToggled;
    }
}
