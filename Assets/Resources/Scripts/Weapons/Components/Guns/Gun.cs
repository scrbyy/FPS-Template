using System;
using UnityEngine;
using Zenject;

using scrbyy.FpsTemplate.Weapons.AttackMethods;

namespace scrbyy.FpsTemplate.Weapons.Guns
{
    public class Gun : Weapon, IAmmoHolder
    {
        public event Action OnReloadStart;
        public event Action<int, int> OnAmmoChanged;

        public int CurrentAmmo => _reloader.CurrentAmmo;
        public int ReserveAmmo => _reloader.ReserveAmmo;

        private GunReloader _reloader;

        [Inject] private AttackMethodFactory _attackMethodFactory;

        public override void Initialize()
        {
            base.Initialize();

            if (_data is not GunData gunData)
            {
                Debug.LogError("Wrong data asset!");
                return;
            }

            if (_attacker == null && _reloader == null)
            {
                _reloader = new GunReloader(gunData);
                _attacker = new GunAttacker(_origin, _reloader.CanShoot, gunData, gunData, _attackMethodFactory);
            }

            _reloader.Initialize();
            _attacker.Initialize();

            _reloader.OnReloadEnd += NotifyUpdateAmmo;
            _reloader.OnReloadStart += NotifyReloadStart;

            SubscribeAttackerEvents();

            NotifyUpdateAmmo();
        }

        public override void Deinitialize()
        {
            base.Deinitialize();

            if (_reloader != null)
            {
                _reloader.Deinitialize();
                _reloader.OnReloadEnd -= NotifyUpdateAmmo;
                _reloader.OnReloadStart -= NotifyReloadStart;
            }

            if (_attacker != null)
            {
                _attacker.Deinitialize();
                UnsubscribeAttackerEvents();
            }
        }

        public void Reload()
        {
            if (!_isOpened) return;
            if (!_attacker.IsAutoAttacking)
            {
                _reloader.Reload();
            }
        }

        protected override void SubscribeAttackerEvents()
        {
            if (_attacker != null && _reloader != null)
            {
                _attacker.OnAttack += NotifyAttack;
                _attacker.OnAttack += _reloader.UseBullet;
                _attacker.OnAttack += NotifyUpdateAmmo;
                _attacker.OnHit += HandleAttackerHit;
            }
        }

        protected override void UnsubscribeAttackerEvents()
        {
            if (_attacker != null && _reloader != null)
            {
                _attacker.OnAttack -= NotifyAttack;
                _attacker.OnAttack -= _reloader.UseBullet;
                _attacker.OnAttack -= NotifyUpdateAmmo;
                _attacker.OnHit -= HandleAttackerHit;
            }
        }

        private void NotifyUpdateAmmo()
        {
            OnAmmoChanged?.Invoke(_reloader.CurrentAmmo, _reloader.ReserveAmmo);
        }

        private void NotifyReloadStart()
        {
            OnReloadStart?.Invoke();
        }
    }
}