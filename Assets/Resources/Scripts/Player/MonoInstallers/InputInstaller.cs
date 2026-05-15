using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    [SerializeField] private NewInputProvider inputProvider;

    public override void InstallBindings()
    {
        Container.Bind<IInputProvider>().FromInstance(inputProvider).AsSingle().NonLazy();
    }
}