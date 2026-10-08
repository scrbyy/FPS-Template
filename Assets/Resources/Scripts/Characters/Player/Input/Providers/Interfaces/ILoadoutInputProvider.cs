using System;

public interface ILoadoutInputProvider
{
    public event Action OnNextWeaponSelected;
    public event Action OnPreviousWeaponSelected;
}