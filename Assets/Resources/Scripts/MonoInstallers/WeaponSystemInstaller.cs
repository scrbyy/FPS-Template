using UnityEngine;
using Zenject;

public class WeaponSystemInstaller : MonoInstaller
{
    [SerializeField] private WeaponInventory _weaponInventory;

    public override void InstallBindings()
    {
        Container.Bind<GunInitializer>().AsSingle();
        Container.BindInstance(_weaponInventory).AsSingle();
    }
}