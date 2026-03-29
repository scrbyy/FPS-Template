using System;
using System.Collections;
using UnityEngine;

public class Pistol : Weapon
{
    public override event Action OnWeaponShoot;

    public override void Shoot()
    {
        if (_canShoot == true && _currentAmmo > 0)
        {
            OnWeaponShoot?.Invoke();
            _currentAmmo--;
            _shootingMethod.ExecuteShoot();
            _afterShootCoroutine = StartCoroutine(ShootCooldown(_afterShootCooldown));
        }
    }
    private IEnumerator ShootCooldown(float duration)
    {
        _canShoot = false;
        yield return new WaitForSeconds(duration);
        _afterShootCoroutine = null;
        _canShoot = true;
    }
}