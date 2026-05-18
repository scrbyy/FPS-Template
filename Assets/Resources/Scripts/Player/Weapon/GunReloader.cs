using UnityEngine;
using System.Collections;
using System;

public class GunReloader : MonoBehaviour 
{
    public event Action OnReload;
    public event Action OnReloadEnd;

    public bool IsReloading => _isReloading;

    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _magazineSize;
    [SerializeField] private int _reserveAmmo;
    [SerializeField] private float _reloadDuration;

    private bool _isReloading = false;

    public void Reload()
    {
        StartCoroutine(ReloadCooldown());
    }

    public void UseBullet()
    {
        _currentAmmo--;
    }

    private IEnumerator ReloadCooldown()
    {
        _isReloading = true;
        yield return new WaitForSeconds(_reloadDuration);
        if (_currentAmmo < _magazineSize)
        {
            OnReload?.Invoke();
            if (_reserveAmmo < _magazineSize)
            {
                if (_reserveAmmo > 0)
                {
                    _currentAmmo = Mathf.Clamp(_currentAmmo += _reserveAmmo, 0, _magazineSize);
                    _reserveAmmo = 0;
                }
            }
            else if (_reserveAmmo >= _magazineSize)
            {
                _reserveAmmo -= _magazineSize - _currentAmmo;
                _currentAmmo = Mathf.Clamp(_currentAmmo += _magazineSize, 0, _magazineSize);
            }
        }
        else if (_reserveAmmo < _magazineSize)
        {
            OnReload?.Invoke();
            if (_reserveAmmo > 0)
            {
                _currentAmmo = Mathf.Clamp(_currentAmmo += _magazineSize, 0, _magazineSize);
            }
        }
        _isReloading = false;
        OnReloadEnd?.Invoke();
    }
}