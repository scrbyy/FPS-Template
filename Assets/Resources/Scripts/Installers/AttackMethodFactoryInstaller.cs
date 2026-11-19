using Zenject;

using scrbyy.FpsTemplate.Weapons.AttackMethods;

namespace scrbyy.FpsTemplate.Services.Installers
{
    public class AttackMethodFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AttackMethodFactory>().AsSingle();
        }
    }
}