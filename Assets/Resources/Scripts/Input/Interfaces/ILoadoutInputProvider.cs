using System;

namespace scrbyy.FpsTemplate.InputSystem.Interfaces
{
    public interface ILoadoutInputProvider
    {
        public event Action OnNextWeaponSelected;
        public event Action OnPreviousWeaponSelected;
    }
}