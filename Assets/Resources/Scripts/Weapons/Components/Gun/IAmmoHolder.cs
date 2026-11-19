using System;

namespace scrbyy.FpsTemplate.Weapons.Guns
{
    public interface IAmmoHolder
    {
        public event Action<int, int> OnAmmoChanged;
        int CurrentAmmo { get; }
        int ReserveAmmo { get; }
    }
}