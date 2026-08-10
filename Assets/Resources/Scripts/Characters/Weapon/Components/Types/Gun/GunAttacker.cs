using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class GunAttacker :WeaponAttacker
{
    public override event Action OnShoot;
    public override event Action<HitData> OnShotContact;

    private Func<bool> _canShootPredicate;

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

    public override async UniTask StartShoot()
    {
        if (_isAttacking) return;

        ResetCts();
        _isAttacking = true;

        try
        {
            while (_isAttacking && _canShootPredicate())
            {
                Shoot();

                await UniTask.Delay(TimeSpan.FromSeconds(_attackData.AfterAttackDelay), cancellationToken: _shootCts.Token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _isAttacking = false;
        }
    }

    protected override void Shoot()
    {
        OnShoot?.Invoke();
        HitData hitData = _attackMethod.Execute();

        if (hitData.isHit)
        {
            OnShotContact?.Invoke(hitData);
            _hitHandler.HandleShot(hitData, CalculateDamageAtDistance(hitData.Distance));
        }
    }

    private float CalculateDamageAtDistance(float distance)
    {
        float damageExponent = distance / _distanceAttackData.DamageDecreasingStep;
        float finalDamage = _attackData.Damage * Mathf.Pow(_distanceAttackData.DistanceDamageMultiplier, damageExponent);
        return finalDamage;
    }
}