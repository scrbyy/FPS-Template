using Zenject;

public class InitializersRegistryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ShootingFeature>().AsTransient();
        Container.Bind<ReloadingFeature>().AsTransient();

        Container.Bind<IWeaponInitializer>().To<GunInitializer>().AsSingle();

        Container.Bind<WeaponInitializersRegistry>().AsSingle();
    }
}