using System;
using System.Collections.Generic;

public class WeaponInitializersRegistry
{
    private readonly Dictionary<Type, IWeaponInitializer> _initializersMap = new();

    public WeaponInitializersRegistry(List<IWeaponInitializer> initializers)
    {
        foreach (var initializer in initializers)
        {
            Type weaponType = initializer.TargetWeaponType;
            _initializersMap[weaponType] = initializer;
        }
    }

    public bool TryGetInitializer(Weapon weapon, out IWeaponInitializer initializer)
    {
        initializer = null;
        if (weapon == null) return false;

        return _initializersMap.TryGetValue(weapon.GetType(), out initializer);
    }
}