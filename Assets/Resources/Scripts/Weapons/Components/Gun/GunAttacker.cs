using System;
using UnityEngine;

using scrbyy.FpsTemplate.Weapons.AttackMethods;
using scrbyy.FpsTemplate.Weapons.Hit;
using scrbyy.FpsTemplate.Weapons.Data.Interfaces;
using scrbyy.FpsTemplate.Weapons.Data;

namespace scrbyy.FpsTemplate.Weapons.Guns
{
    public class GunAttacker : WeaponAttacker
    {
        private readonly Func<bool> _canShootPredicate;
        private readonly IDistanceAttackData _distanceAttackData;

        public GunAttacker(
            Transform origin,
            Func<bool> canShootPredicate,
            IDistanceAttackData distanceAttackData,
            WeaponData weaponData,
            AttackMethodFactory attackFactory)
            : base(weaponData.GetAttackConfig(), origin, weaponData, attackFactory)
        {
            _distanceAttackData = distanceAttackData;
            _canShootPredicate = canShootPredicate;
        }

        protected override bool CanAttack()
        {
            return base.CanAttack() && (_canShootPredicate == null || _canShootPredicate());
        }

        protected override float CalculateDamage(HitData hitData)
        {
            if (_distanceAttackData == null || _distanceAttackData.DamageDecreasingStep <= 0)
                return _attackData.Damage;

            float damageExponent = hitData.Distance / _distanceAttackData.DamageDecreasingStep;
            return _attackData.Damage * Mathf.Pow(_distanceAttackData.DistanceDamageMultiplier, damageExponent);
        }
    }
}