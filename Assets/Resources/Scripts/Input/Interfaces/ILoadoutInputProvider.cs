using System;

namespace scrbyy.FpsTemplate.Input
{
    public interface ILoadoutInputProvider
    {
        public event Action OnNextWeaponSelected;
        public event Action OnPreviousWeaponSelected;
    }
}