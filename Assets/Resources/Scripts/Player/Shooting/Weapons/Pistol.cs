using System;
using System.Collections;
using UnityEngine;

public class Pistol : Weapon
{
    public override event Action OnWeaponShoot;

    public override void Shoot()
    {
        if (canShoot == true && currentAmmo > 0)
        {
            OnWeaponShoot?.Invoke();
            currentAmmo--;
            if (Physics.Raycast(origin.position, origin.forward, out hit, distance))
            {
                GameObject hitObject = hit.transform.gameObject;
                Debug.Log("Pistol hit object: " + hitObject.name);
            }
            afterShootDelay = StartCoroutine(ShootCooldown(afterShootCooldown));
        }
    }
    private IEnumerator ShootCooldown(float duration)
    {
        canShoot = false;
        yield return new WaitForSeconds(duration);
        afterShootDelay = null;
        canShoot = true;
    }
}