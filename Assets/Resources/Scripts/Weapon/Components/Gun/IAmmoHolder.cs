using System;

public interface IAmmoHolder
{
    public event Action<int, int> OnAmmoChanged;
    int CurrentAmmo { get; }
    int ReserveAmmo { get; }
}