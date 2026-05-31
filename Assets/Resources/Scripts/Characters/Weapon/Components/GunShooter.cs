using System;
using UnityEngine;

public class GunShooter
{
    public event Action OnShoot;

    Transform _origin;

    private float _damage;

    private float _distance;
    private float _distanceModifier;

    private float _decreasingStep;

    private HitHandler _hitHandler;
    private IShootingMethod _shootingMethod;

    private GameObject _decal;

    public void Shoot()
    {
        OnShoot?.Invoke();
        HitData hitData = _shootingMethod.ExecuteShoot();

        if (hitData.isHit)
        {
            _hitHandler.HadleShot(hitData, CalculateDamageAtDistance(hitData.Distance), _decal);
        }
    }

    public float CalculateDamageAtDistance(float distance)
    {
        float damageExponent = distance / _decreasingStep;
        float finalDamage = _damage * Mathf.Pow(_distanceModifier, damageExponent);
        return finalDamage;
    }

    public void Initialize(IShootingData shootingData, Transform origin)
    {
        _origin = origin;
        _distance = shootingData.Distance;
        _damage = shootingData.Damage;
        _distanceModifier = shootingData.DistanceModifier;
        _decreasingStep = shootingData.DamageDecreasingStep;
        _decal = shootingData.Decal;

        _hitHandler = new HitHandler();
        _shootingMethod = new ShootingMethodFactory(_origin, _distance).CreateShootingMethod(shootingData.ShootingMethod);
    }
}