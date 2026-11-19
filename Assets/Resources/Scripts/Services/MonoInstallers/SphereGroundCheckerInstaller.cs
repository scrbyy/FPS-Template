using Zenject;
using UnityEngine;

using scrbyy.FpsTemplate.Characters.Player.Movement.GroundChecker;

namespace scrbyy.FpsTemplate.Services.MonoInstallers
{
    public class SphereGroundCheckerInstaller : MonoInstaller
    {
        [SerializeField] private SphereGroundChecker _groundChecker;
        public override void InstallBindings()
        {
            Container.Bind<IGroundChecker>().FromInstance(_groundChecker).AsSingle();
        }
    }
}