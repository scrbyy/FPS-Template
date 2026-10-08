using System;
using UnityEngine;

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

    protected override bool CanShoot()
    {
        return base.CanShoot() && (_canShootPredicate == null || _canShootPredicate());
    }

    protected override float CalculateDamage(HitData hitData)
    {
        if (_distanceAttackData == null || _distanceAttackData.DamageDecreasingStep <= 0)
            return _attackData.Damage;

        float damageExponent = hitData.Distance / _distanceAttackData.DamageDecreasingStep;
        return _attackData.Damage * Mathf.Pow(_distanceAttackData.DistanceDamageMultiplier, damageExponent);
    }
}