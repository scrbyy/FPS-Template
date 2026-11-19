using System;

namespace scrbyy.FpsTemplate.Weapons.Initializers 
{
    public interface IWeaponInitializer
    {
        Type TargetWeaponType { get; }
        void Select(Weapon weapon);
        void Unselect(Weapon weapon);
    }
}