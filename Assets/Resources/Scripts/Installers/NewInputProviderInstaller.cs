using UnityEngine;
using Zenject;

using scrbyy.FpsTemplate.InputSystem;
using scrbyy.FpsTemplate.InputSystem.Interfaces;

namespace scrbyy.FpsTemplate.Installers
{
    public class NewInputProviderInstaller : MonoInstaller
    {
        [SerializeField] private NewInputSystemProvider _inputProvider;

        public override void InstallBindings()
        {
            Container.Bind<IMovementInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
            Container.Bind<IInteractionInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
            Container.Bind<IWeaponInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
            Container.Bind<ILoadoutInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
            Container.Bind<ILookInputProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
            Container.Bind<IUIMenuProvider>().FromInstance(_inputProvider).AsSingle().NonLazy();
        }
    }
}