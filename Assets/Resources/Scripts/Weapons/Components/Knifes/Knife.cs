using Zenject;
using UnityEngine;

using scrbyy.FpsTemplate.Characters.Stats;
using scrbyy.FpsTemplate.Weapons.AttackMethods;

namespace scrbyy.FpsTemplate.Weapons.Knifes
{
    public class Knife : Weapon
    {
        [SerializeField] private CharacterStamina _characterStamina;

        [Inject] private AttackMethodFactory _attackMethodFactory;

        private KnifeData _knifeData => _data as KnifeData;

        public override void Initialize()
        {
            base.Initialize();

            _attacker = new KnifeAttacker(_data.GetAttackConfig(), _origin, _data, _attackMethodFactory);
            _attacker.Initialize();
            SubscribeAttackerEvents();
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
            _attacker.Deinitialize();
            UnsubscribeAttackerEvents();
        }

        public override void Attack()
        {
            if(!_characterStamina.IsEnoughStamina(_knifeData.StaminaCost)) return;
            if(!_isOpened) return;
            base.Attack();
            _characterStamina.Decrease(_knifeData.StaminaCost);
        }
    }
}