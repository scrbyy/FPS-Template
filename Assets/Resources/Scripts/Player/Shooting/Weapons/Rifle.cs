using System.Collections;
using UnityEngine;

public class Rifle : Weapon
{
    public override void Shoot()
    {
        if (canShoot == true && currentAmmo > 0)
        {
            currentAmmo--;
            if (Physics.Raycast(origin.position, origin.forward, out hit, distance))
            {
                GameObject hitObject = hit.transform.gameObject;
                Debug.Log("Hit object: " + hitObject.name);
            }
            afterShootDelay = StartCoroutine(ShootCooldown(shootCooldown));
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