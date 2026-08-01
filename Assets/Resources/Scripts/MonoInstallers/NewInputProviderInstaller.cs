using UnityEngine;
using Zenject;

public class NewInputProviderInstaller : MonoInstaller
{
    [SerializeField] private NewInputProvider _inputProvider;

    public override void InstallBindings()
    {
        Container.Bind<IMovementInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
        Container.Bind<IInteractionInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
        Container.Bind<IWeaponInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
        Container.Bind<ILoadoutInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
        Container.Bind<ILookInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
    }
}