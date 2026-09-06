using UnityEngine;
using Zenject;

public class GameFSMInstaller : MonoInstaller
{
    [SerializeField] private GameFSM _gameFSM;

    public override void InstallBindings()
    {
        Container.Bind<IState>().To<PlayState>().AsSingle();
        Container.Bind<IState>().To<PlayerDeadState>().AsSingle();

        Container.Bind<GameFSM>().FromInstance(_gameFSM).AsSingle().NonLazy();
    }
}