using System;
using UnityEngine;
using System.Collections;

public class GunShooter : MonoBehaviour
{
    public event Action OnShoot;

    public bool IsShooting => _isShooting;

    [SerializeField] private float _afterShootDelay;
    [SerializeField] private RecoilType _recoilType;

    [SerializeField] private Transform _origin;
    [SerializeField] private float _distance;

    private IShootingMethod _shootingMethod;

    private Coroutine _shootDelayCoroutine;

    private bool _isShooting = false;


    public void Shoot()
    {
        if (_isShooting == false)
        {
            OnShoot?.Invoke();
            _shootingMethod.ExecuteShoot();
            _shootDelayCoroutine = StartCoroutine(ShootDelay(_afterShootDelay));
        }
    }

    private IEnumerator ShootDelay(float duration)
    {
        _isShooting = true;
        yield return new WaitForSeconds(duration);

        _isShooting = false;
        _shootDelayCoroutine = null;
    }
}