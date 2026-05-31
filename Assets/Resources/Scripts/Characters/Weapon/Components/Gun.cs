using System;
using System.Collections;
using UnityEngine;

public class Gun : Weapon, IShootable
{
    [SerializeField] private Transform _origin;

    [SerializeField] private GunData _gunData;

    [SerializeField] private GunReloader _reloader;

    private GunShooter _shooter;

    public RecoilType RecoilType => _gunData.RecoilType;

    public int CurrentAmmo => _reloader.CurrentAmmo;

    public int ReserveAmmo => _reloader.ReserveAmmo;

    public event Action<int, int> OnAmmoChanged;

    private Coroutine _coroutine = null;
    private bool _isShooting = false;

    public override void Attack()
    {
        if (_coroutine == null)
        {
            _isShooting = true;
            _coroutine = StartCoroutine(AutomaticShoot());
        }
    }

    public void StopAttack()
    {
        if (_coroutine != null)
        {
            _isShooting = false;
            StopCoroutine(_coroutine);
            _coroutine = null;
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
        _coroutine = null;
        _isShooting = false;
    }

    public void Reload()
    {
        if (_isShooting == false)
        {
            _reloader.Reload();
        }
    }

    public void Awake()
    {
        _shooter = new GunShooter();
        _reloader.Initialize(_gunData);
        _shooter.Initialize(_gunData, _origin);
        NotifyUpdateAmmo();
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