using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public event System.Action<int, int> OnEndReloadEvent;
    public event System.Action OnWeaponShoot;

    [Header("Damage")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _afterShootCooldown;
    [SerializeField] private RecoilType _recoilType;

    [Header("Ammo")]
    [SerializeField] protected int _currentAmmo;
    [SerializeField] protected int _reserveAmmo;
    [SerializeField] protected int _magazineSize;
    [SerializeField] protected float _reloadDuration;

    [Header("Ray")]
    [SerializeField] protected Transform _origin;
    [SerializeField] protected float _distance;

    protected IShootingMethod _shootingMethod;
    protected bool _canShoot = true;
    protected Coroutine _afterShootCoroutine;
    protected Coroutine _reloadCoroutine;
    protected RaycastHit _hit;

    public void Shoot()
    {
        if (_canShoot == true && _currentAmmo > 0)
        {
            OnWeaponShoot?.Invoke();
            _currentAmmo--;
            _shootingMethod.ExecuteShoot();
            _afterShootCoroutine = StartCoroutine(ShootCooldown(_afterShootCooldown));
        }
    }
    public RecoilType GetRecoilType() 
    {
        return _recoilType;
    }
    public int GetCurrentAmmo()
    {
        return _currentAmmo;
    }

    public void SetShootingMethod(IShootingMethod shootingMethod)
    {
        _shootingMethod = shootingMethod;
    }

    public int GetReserveAmmo()
    {
        return _reserveAmmo;
    }

    public Transform GetShootOrigin()
    {
        return _origin;
    }

    public float GetShootDistance()
    {
        return _distance;
    }

    public void Reload()
    {
        if (_canShoot == true && _reserveAmmo > 0 && _currentAmmo != _magazineSize)
        {
            _reloadCoroutine = StartCoroutine(ReloadCooldown(_reloadDuration));
        }
    }

    public void Disable()
    {
        if(_afterShootCoroutine != null) StopCoroutine(_afterShootCoroutine);
        _afterShootCoroutine = null;

        if (_reloadCoroutine != null) StopCoroutine(_reloadCoroutine);
        _reloadCoroutine = null;
    }

    private IEnumerator ReloadCooldown(float duration)
    {
        _canShoot = false;
        yield return new WaitForSeconds(duration);
        if (_currentAmmo < _magazineSize)
        {
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
            if (_reserveAmmo > 0)
            {
                _currentAmmo = Mathf.Clamp(_currentAmmo += _magazineSize, 0, _magazineSize);
            }
        }
        OnEndReloadEvent?.Invoke(_currentAmmo, _reserveAmmo);
        _canShoot = true;
        _reloadCoroutine = null;
    }

    private IEnumerator ShootCooldown(float duration)
    {
        _canShoot = false;
        yield return new WaitForSeconds(duration);
        _afterShootCoroutine = null;
        _canShoot = true;
    }
}