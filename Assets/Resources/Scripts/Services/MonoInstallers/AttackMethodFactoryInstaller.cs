using Zenject;

using scrbyy.FpsTemplate.Weapons.AttackMethods;

namespace scrbyy.FpsTemplate.Services.MonoInstallers
{
    public class AttackMethodFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AttackMethodFactory>().AsSingle();
        }
    }
}