using Zenject;

using scrbyy.FpsTemplate.Weapons.AttackMethods;

public class AttackMethodFactoryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AttackMethodFactory>().AsSingle();
    }
}