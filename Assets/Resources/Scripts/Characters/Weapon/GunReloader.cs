using UnityEngine;
using System.Collections;
using System;

public class GunReloader : MonoBehaviour 
{
    public event Action OnReload;
    public event Action OnReloadEnd;

    public int CurrentAmmo => _currentAmmo;
    public int ReserveAmmo => _reserveAmmo;
    public bool IsReloading => _isReloading;

    private int _currentAmmo;
    private int _magazineSize;
    private int _reserveAmmo;
    private float _reloadDuration;

    private bool _isReloading;

    private Coroutine _reloadCoroutine;

    public void Initialize(IAmmoData ammoData)
    {
        _currentAmmo = ammoData.StartAmmo;
        _magazineSize = ammoData.MagazineSize;
        _reserveAmmo = ammoData.ReserveAmmo;
        _reloadDuration = ammoData.ReloadDuration;

        _isReloading = false;
    }

    public void Deinitialize()
    {
        if (_reloadCoroutine != null)
        {
            StopCoroutine(_reloadCoroutine);
            _reloadCoroutine = null;
            _isReloading = false;
        }
    }

    public void Reload()
    {
        if(_reloadCoroutine == null)
        {
            if (_reserveAmmo > 0)
            {
                if(_currentAmmo < _magazineSize)
                {
                    _reloadCoroutine = StartCoroutine(ReloadCooldown());
                }
            }
        }
    }

    public void UseBullet()
    {
        _currentAmmo--;
    }

    public bool CanShoot()
    {
        return _currentAmmo > 0;
    }

    private IEnumerator ReloadCooldown()
    {
        OnReload?.Invoke();
        _isReloading = true;

        yield return new WaitForSeconds(_reloadDuration);

        int neededAmmo = _magazineSize - _currentAmmo;

        if (_reserveAmmo > neededAmmo)
        {
            _currentAmmo += neededAmmo;
            _reserveAmmo -= neededAmmo;
        }
        else
        {
            _currentAmmo += _reserveAmmo;
            _reserveAmmo = 0;
        }
        OnReloadEnd?.Invoke();
        _isReloading = false;
        _reloadCoroutine = null;
    }
}