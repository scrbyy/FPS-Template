using Zenject;

public class InitializersRegistryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AttackBinder>().AsTransient();
        Container.Bind<ReloadingBinder>().AsTransient();

        Container.Bind<IWeaponInitializer>().To<GunInitializer>().AsSingle();
        Container.Bind<IWeaponInitializer>().To<KnifeInitializer>().AsSingle();

        Container.Bind<WeaponInitializersRegistry>().AsSingle();
    }
}