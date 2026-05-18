using System;
using System.Collections;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public event Action OnShoot;

    public bool IsShooting => _isShooting;

    [SerializeField] private Transform _origin;

    private float _afterShotDelay;
    private float _distance;
    private IShootingMethod _shootingMethod;
    private Coroutine _shootDelayCoroutine;
    private bool _isShooting;


    public void Shoot()
    {
        OnShoot?.Invoke();
        _shootingMethod.ExecuteShoot();
        _shootDelayCoroutine = StartCoroutine(ShootDelay(_afterShotDelay));
    }

    private IEnumerator ShootDelay(float duration)
    {
        _isShooting = true;
        yield return new WaitForSeconds(duration);

        _isShooting = false;
        _shootDelayCoroutine = null;
    }

    public void Initialize(IShootingData shootingData)
    {
        _afterShotDelay = shootingData.AfterShotDelay;
        _distance = shootingData.Distance;
        _isShooting = false;

        _shootingMethod = new ShootingMethodFactory(_origin, _distance).CreateShootingMethod(shootingData.ShootingMethod);
    }
}