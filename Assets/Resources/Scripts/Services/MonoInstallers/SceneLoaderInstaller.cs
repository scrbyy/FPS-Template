using Zenject;

using scrbyy.FpsTemplate.Services.GameCycle;

public class SceneLoaderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneLoader>().AsSingle();
    }
}