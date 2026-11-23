using scrbyy.FpsTemplate.InputSystem;
using Zenject;

public class InputAssetInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputActionsAsset>().AsSingle().NonLazy();
    }
}
