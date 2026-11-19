using Zenject;
using Cysharp.Threading.Tasks;

using scrbyy.FpsTemplate.Weapons.AttackMethods;

namespace scrbyy.FpsTemplate.Weapons
{

    public class Knife : Weapon
    {
        [Inject] private AttackMethodFactory _attackMethodFactory;

        public override void Initialize()
        {
            base.Initialize();
            _weaponAttacker = new KnifeAttacker(_data.GetAttackConfig(), _origin, _data, _attackMethodFactory);
            OpenDelay(_data.OpenDelay).Forget();
            _weaponAttacker.Initialize();
            SubscribeAttackerEvents();
        }

        public override void Deinitialize()
        {
            UnsubscribeAttackerEvents();
            base.Deinitialize();
            _weaponAttacker.Deinitialize();
        }
    }
}