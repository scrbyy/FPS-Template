using System;
using UnityEngine;
using System.Collections;

public class GunShooter : MonoBehaviour
{
    public event Action OnShoot;

    public bool IsShooting => _isShooting;

    [SerializeField] private Transform _origin;

    private float _damage;

    private float _distance;
    private float _distanceModifier;

    private bool _isShooting;
    private float _afterShotDelay;
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

        StartCoroutine(ShootDelay(_afterShotDelay));
    }

    private IEnumerator ShootDelay(float duration)
    {
        _isShooting = true;
        yield return new WaitForSeconds(duration);

        _isShooting = false;
    }

    public float CalculateDamageAtDistance(float distance)
    {
        float damageExponent = distance / _decreasingStep;
        float finalDamage = _damage * Mathf.Pow(_distanceModifier, damageExponent);
        return finalDamage;
    }

    public void Initialize(IShootingData shootingData)
    {
        _afterShotDelay = shootingData.AfterShotDelay;
        _distance = shootingData.Distance;
        _damage = shootingData.Damage;
        _isShooting = false;
        _distanceModifier = shootingData.DistanceModifier;
        _decreasingStep = shootingData.DamageDecreasingStep;
        _decal = shootingData.Decal;

        _hitHandler = new HitHandler();
        _shootingMethod = new ShootingMethodFactory(_origin, _distance).CreateShootingMethod(shootingData.ShootingMethod);
    }
}