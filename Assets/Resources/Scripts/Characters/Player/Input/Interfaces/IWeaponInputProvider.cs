using System;

public interface IWeaponInputProvider
{
    public event Action OnShootStarted;
    public event Action OnShootPressed;

    public event Action OnReloadStarted;
}