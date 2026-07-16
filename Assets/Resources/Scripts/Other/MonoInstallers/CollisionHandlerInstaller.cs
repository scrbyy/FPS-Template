using Zenject;
using UnityEngine;
public class CollisionHandlerInstaller : MonoInstaller
{
    [SerializeField] private CharacterController _characterController;

    public override void InstallBindings()
    {
        Container.Bind<CharacterCollisionHandler>().AsSingle().WithArguments(_characterController);
    }
}