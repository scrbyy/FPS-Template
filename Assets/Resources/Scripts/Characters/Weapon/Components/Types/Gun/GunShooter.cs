using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class GunShooter
{
    public event Action OnShoot;
    public event Action<HitData> OnShotContact;

    public bool IsShooting => _isShooting;

    private Transform _origin;

    private float _damage;

    private float _distance;
    private float _distanceModifier;

    private float _decreasingStep;

    private HitHandler _hitHandler;
    private IAttackMethod _shootingMethod;

    private float _afterShotDelay;

    private bool _isShooting;

    private Func<bool> _canShootPredicate;

    private CancellationTokenSource _shootCts;

    public GunShooter(IShootingData shootingData, Transform origin, Func<bool> canShootPredicate)
    {
        _origin = origin;
        _distance = shootingData.MaxDistance;
        _damage = shootingData.Damage;
        _distanceModifier = shootingData.DistanceDamageMultiplier;
        _decreasingStep = shootingData.DamageDecreasingStep;
        _afterShotDelay = shootingData.AfterAttackDelay;
        _canShootPredicate = canShootPredicate;

        _hitHandler = new HitHandler();
        _shootingMethod = new AttackMethodFactory(_origin, _distance).CreateAttackMethod(shootingData.AttackMethod);
    }

    public void Initialize()
    {
        _shootCts = new CancellationTokenSource();
    }

    public void Deinitialize()
    {
        StopShoot();
        _shootCts?.Dispose();
    }

    public async UniTask StartShoot()
    {
        if (_isShooting) return;

        ResetCts();
        _isShooting = true;

        try
        {
            while (_isShooting && _canShootPredicate())
            {
                Shoot();

                await UniTask.Delay(TimeSpan.FromSeconds(_afterShotDelay), cancellationToken: _shootCts.Token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _isShooting = false;
        }
    }

    public void StopShoot()
    {
        if(_isShooting)
        {
            _isShooting = false;
            _shootCts?.Cancel();
        }
    }

    private void ResetCts()
    {
        _shootCts?.Cancel();
        _shootCts?.Dispose();
        _shootCts = new CancellationTokenSource();
    }

    private void Shoot()
    {
        OnShoot?.Invoke();
        HitData hitData = _shootingMethod.Execute();

        if (hitData.isHit)
        {
            OnShotContact?.Invoke(hitData);
            _hitHandler.HandleShot(hitData, CalculateDamageAtDistance(hitData.Distance));
        }
    }

    private float CalculateDamageAtDistance(float distance)
    {
        float damageExponent = distance / _decreasingStep;
        float finalDamage = _damage * Mathf.Pow(_distanceModifier, damageExponent);
        return finalDamage;
    }
}