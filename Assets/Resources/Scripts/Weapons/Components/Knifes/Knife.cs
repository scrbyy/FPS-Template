using Cysharp.Threading.Tasks;
using scrbyy.FpsTemplate.Characters.Stats;
using scrbyy.FpsTemplate.Weapons.AttackMethods;
using scrbyy.FpsTemplate.Weapons.Guns;
using UnityEngine;
using Zenject;

namespace scrbyy.FpsTemplate.Weapons.Knifes
{
    public class Knife : Weapon
    {
        [Inject] private AttackMethodFactory _attackMethodFactory;
        [SerializeField] private CharacterStamina _characterStamina;    

        private KnifeData _knifeData => _data as KnifeData;

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

        public override void Attack()
        {
            if(!_characterStamina.IsEnoughStamina(_knifeData.StaminaCost)) return;

            base.Attack();
            _characterStamina.Decrease(_knifeData.StaminaCost);
        }
    }
}