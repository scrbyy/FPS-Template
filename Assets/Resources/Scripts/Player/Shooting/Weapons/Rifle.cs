using System.Collections;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField] private float shootCooldown;
    [SerializeField] private float distance;
    private Coroutine CooldownCorutine;

    public override void Reload()
    {
        if (CooldownCorutine == null)
        {
            CooldownCorutine = StartCoroutine(ReloadCooldown(reloadDuration));
        }
    }
    public override void Shoot()
    {
        if (CooldownCorutine == null && currentAmmo > 0)
        {
            currentAmmo--;
            if (Physics.Raycast(origin.position, transform.forward, out hit, distance))
            {
                GameObject hitObject = hit.transform.gameObject;
                Debug.Log("Hit object: " + hitObject.name);
            }
            CooldownCorutine = StartCoroutine(ShootCooldown(shootCooldown));
        }
    }

    private IEnumerator ShootCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        CooldownCorutine = null;
    }

    private IEnumerator ReloadCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (currentAmmo < magazineSize)
        {
            if (reserveAmmo < magazineSize)
            {
                if (reserveAmmo > 0)
                {
                    currentAmmo = Mathf.Clamp(currentAmmo += reserveAmmo, 0, magazineSize);
                    reserveAmmo = 0;
                }
            }
            else if(reserveAmmo >= magazineSize)
            {
                reserveAmmo -= magazineSize - currentAmmo;
                currentAmmo = Mathf.Clamp(currentAmmo += magazineSize, 0, magazineSize);
            }
        }
        else if (reserveAmmo < magazineSize)
        {
            if (reserveAmmo > 0)
            {
                currentAmmo = Mathf.Clamp(currentAmmo += magazineSize, 0, magazineSize);
            }
        }
            EndReloadEvent?.Invoke(currentAmmo, reserveAmmo);
        CooldownCorutine = null;
    }
}