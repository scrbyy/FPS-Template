using UnityEngine;
using Zenject;

public class GroundCheckerInstaller : MonoInstaller
{
    [SerializeField] private GroundChecker _groundChecker;

    public override void InstallBindings()
    {
        Container.Bind<GroundChecker>().FromInstance(_groundChecker).AsSingle().NonLazy();
    }
}
