using System;

public interface IShootable
{
    public event Action<int, int> OnAmmoChanged;
    int CurrentAmmo { get; }
    int ReserveAmmo { get; }
}