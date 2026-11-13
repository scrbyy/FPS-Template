using Zenject;

public class AttackMethodFactoryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AttackMethodFactory>().AsSingle();
    }
}