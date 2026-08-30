using System;

public interface IWeaponInputProvider
{
    public event Action OnShootReleased;
    public event Action OnShootStarted;

    public event Action OnReloadStarted;
}