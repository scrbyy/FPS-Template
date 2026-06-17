using System;
using System.Collections;
using UnityEngine;

public class Gun : Weapon, IShootable
{
    public event Action<int, int> OnAmmoChanged;

    public int CurrentAmmo => _reloader.CurrentAmmo;
    public int ReserveAmmo => _reloader.ReserveAmmo;

    public RecoilType RecoilType => _gunData.RecoilType;

    [SerializeField] private Transform _origin;
    [SerializeField] private GunData _gunData;
    [SerializeField] private GunReloader _reloader;

    private GunShooter _shooter;

    private Coroutine _shootingCoroutine = null;
    private bool _isShooting = false;

    public override void Initialize()
    {
        _shooter = new GunShooter();
        _reloader.Initialize(_gunData);
        _shooter.Initialize(_gunData, _origin);


        NotifyUpdateAmmo();
    }

    public override void Deinitialize()
    {
        _reloader.Deinitialize();
        _shooter.Deinitialize();
        _isShooting = false;
        _shootingCoroutine = null;
    }

    public override void Attack()
    {
        if (_shootingCoroutine == null)
        {
            _isShooting = true;
            _shootingCoroutine = StartCoroutine(AutomaticShoot());
        }
    }

    public void StopAttack()
    {
        if (_shootingCoroutine != null)
        {
            _isShooting = false;
            StopCoroutine(_shootingCoroutine);
            _shootingCoroutine = null;
        }
    }

    public void Reload()
    {
        if (_isShooting == false)
        {
            _reloader.Reload();
        }
    }

    private IEnumerator AutomaticShoot()
    {
        while (_isShooting)
        {
            if (_reloader.IsReloading == false && _reloader.CanShoot())
            {
                _reloader.UseBullet();
                _shooter.Shoot();
                NotifyUpdateAmmo();
                OnAttack?.Invoke();
                yield return new WaitForSeconds(_gunData.AfterShotDelay);
            }
            else break;
        }
        _shootingCoroutine = null;
        _isShooting = false;
    }

    private void NotifyUpdateAmmo()
    {
        OnAmmoChanged?.Invoke(_reloader.CurrentAmmo, _reloader.ReserveAmmo);
    }

    private void OnEnable()
    {
        _reloader.OnReloadEnd += NotifyUpdateAmmo;
    }

    private void OnDisable()
    {
        _reloader.OnReloadEnd -= NotifyUpdateAmmo;
    }
}