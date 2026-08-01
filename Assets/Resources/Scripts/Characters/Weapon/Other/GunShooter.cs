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
    private IShootingMethod _shootingMethod;

    private float _afterShotDelay;

    private GameObject _decal;

    private bool _isShooting;

    private Func<bool> _canShootPredicate;

    private CancellationTokenSource _shootCts;

    public GunShooter(IShootingData shootingData, Transform origin, Func<bool> canShootPredicate)
    {
        _origin = origin;
        _distance = shootingData.Distance;
        _damage = shootingData.Damage;
        _distanceModifier = shootingData.DistanceModifier;
        _decreasingStep = shootingData.DamageDecreasingStep;
        _decal = shootingData.Decal;
        _afterShotDelay = shootingData.AfterShotDelay;
        _canShootPredicate = canShootPredicate;

        _hitHandler = new HitHandler();
        _shootingMethod = new ShootingMethodFactory(_origin, _distance).CreateShootingMethod(shootingData.ShootingMethod);
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
        HitData hitData = _shootingMethod.ExecuteShoot();

        if (hitData.isHit)
        {
            OnShotContact?.Invoke(hitData);
            _hitHandler.HandleShot(hitData, CalculateDamageAtDistance(hitData.Distance), _decal);
        }
    }

    private float CalculateDamageAtDistance(float distance)
    {
        float damageExponent = distance / _decreasingStep;
        float finalDamage = _damage * Mathf.Pow(_distanceModifier, damageExponent);
        return finalDamage;
    }
}