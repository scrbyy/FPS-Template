using Zenject;
using UnityEngine;

namespace scrbyy.FpsTemplate.Services.MonoInstallers
{
    public class CollisionHandlerInstaller : MonoInstaller
    {
        [SerializeField] private CharacterController _characterController;

        public override void InstallBindings()
        {
            Container.Bind<CharacterCollisionHandler>().AsSingle().WithArguments(_characterController);
        }
    }
}