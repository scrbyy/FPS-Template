using Zenject;

using scrbyy.FpsTemplate.Services.GameCycle;

namespace scrbyy.FpsTemplate.Services.MonoInstallers
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
        }
    }
}