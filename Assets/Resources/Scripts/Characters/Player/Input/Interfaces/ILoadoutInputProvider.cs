using System;

public interface ILoadoutInputProvider
{
    public event Action OnNextWeaponSelect;
    public event Action OnPreviousWeaponSelect;
}