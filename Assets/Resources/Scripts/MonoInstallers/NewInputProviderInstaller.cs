using UnityEngine;
using Zenject;

public class NewInputProviderInstaller : MonoInstaller
{
    [SerializeField] private NewInputProvider _inputProvider;

    public override void InstallBindings()
    {
        Container.Bind<IInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
    }
}