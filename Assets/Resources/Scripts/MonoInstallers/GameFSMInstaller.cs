using UnityEngine;
using Zenject;

public class GameFSMInstaller : MonoInstaller
{
    [SerializeField] private GameFSM _gameFSM;

    public override void InstallBindings()
    {
        Container.Bind<IGameState>().To<GameplayState>().AsSingle();
        Container.Bind<IGameState>().To<PauseState>().AsSingle();

        Container.Bind<GameFSM>().FromInstance(_gameFSM).AsSingle().NonLazy();
    }
}