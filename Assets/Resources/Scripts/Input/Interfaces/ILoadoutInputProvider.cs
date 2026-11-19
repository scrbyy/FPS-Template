using System;

namespace scrbyy.FpsTemplate.InputSystem
{
    public interface ILoadoutInputProvider
    {
        public event Action OnNextWeaponSelected;
        public event Action OnPreviousWeaponSelected;
    }
}