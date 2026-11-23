using scrbyy.FpsTemplate.InputSystem;
using UnityEngine;
using Zenject;

public class MapChangerInstaller : MonoInstaller
{
    [SerializeField] private ActionMapChanger _actionMapChanger;

    public override void InstallBindings()
    {
        Container.Bind<ActionMapChanger>().FromInstance(_actionMapChanger).AsSingle().NonLazy();
    }
}