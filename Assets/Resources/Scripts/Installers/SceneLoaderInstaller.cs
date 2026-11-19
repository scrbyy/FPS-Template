using Zenject;

using scrbyy.FpsTemplate.Services.GameCycle;

namespace scrbyy.FpsTemplate.Installers
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
        }
    }
}