using Zenject;
using UnityEngine;

using scrbyy.FpsTemplate.Characters.Player.Movement.Collisions;

namespace scrbyy.FpsTemplate.Services.Installers
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