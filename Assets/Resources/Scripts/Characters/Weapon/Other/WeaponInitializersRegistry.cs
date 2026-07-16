using System;
using System.Collections.Generic;
using Zenject;

public class WeaponInitializersRegistry
{
    private readonly Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();

    private readonly Dictionary<Type, IWeaponInitializer> _instancesCache = new Dictionary<Type, IWeaponInitializer>();

    private readonly DiContainer _container;

   [Inject]
    public WeaponInitializersRegistry(DiContainer container)
    {
        _container = container;

        Register(typeof(Gun), typeof(GunInitializer));
    }

    public void Register(Type weaponType, Type initializerType)
    {
        if (weaponType == null) throw new ArgumentNullException(nameof(weaponType));
        if (initializerType == null) throw new ArgumentNullException(nameof(initializerType));

        if (!_bindings.ContainsKey(weaponType))
        {
            _bindings.Add(weaponType, initializerType);
        }
    }

    public bool TryGetInitializer(Weapon weapon, out IWeaponInitializer initializer)
    {
        initializer = null;
        if (weapon == null) return false;

        Type weaponType = weapon.GetType();

        if (_instancesCache.TryGetValue(weaponType, out initializer))
        {
            return true;
        }

        if (_bindings.TryGetValue(weaponType, out Type initializerType))
        {
            initializer = (IWeaponInitializer)_container.Instantiate(initializerType);

            _instancesCache.Add(weaponType, initializer);
            return true;
        }

        return false;
    }

    public void Clear()
    {
        _bindings.Clear();
        _instancesCache.Clear();
    }
}