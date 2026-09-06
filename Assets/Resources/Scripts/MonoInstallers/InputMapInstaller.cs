using Zenject;

public class InputMapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputSettings>().AsSingle();

        Container.Bind<InputMapSelector>().AsSingle();
    }
}