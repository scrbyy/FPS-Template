using Zenject;

public class InitializersRegistryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AttackFeature>().AsTransient();
        Container.Bind<ReloadingFeature>().AsTransient();

        Container.Bind<IWeaponInitializer>().To<GunInitializer>().AsSingle();
        Container.Bind<IWeaponInitializer>().To<KnifeInitializer>().AsSingle();

        Container.Bind<WeaponInitializersRegistry>().AsSingle();
    }
}