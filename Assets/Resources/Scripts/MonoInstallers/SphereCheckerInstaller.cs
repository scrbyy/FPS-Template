using UnityEngine;
using Zenject;

public class SphereCheckerInstaller : MonoInstaller
{
    [SerializeField] private SphereGroundChecker _groundChecker;

    public override void InstallBindings()
    {
        Container.Bind<IGroundChecker>().FromInstance(_groundChecker).AsSingle().NonLazy();
    }
}
