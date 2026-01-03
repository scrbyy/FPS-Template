using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public System.Action<int, int> EndReloadEvent;

    [SerializeField] protected float damage;
    [SerializeField] protected Transform origin;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected int reserveAmmo;
    [SerializeField] protected int magazineSize;
    [SerializeField] protected float reloadDuration;
    [SerializeField] public ShootType shootType;
    [SerializeField] protected float shootCooldown;
    [SerializeField] protected float distance;

    protected bool canShoot = true;
    protected Coroutine afterShootDelay;
    protected Coroutine reloadCoroutine;
    protected RaycastHit hit;

    public abstract void Shoot();

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetReserveAmmo()
    {
        return reserveAmmo;
    }

    public void Reload()
    {
        if (canShoot == true && reserveAmmo > 0 && currentAmmo != magazineSize)
        {
            reloadCoroutine = StartCoroutine(ReloadCooldown(reloadDuration));
        }
    }

    public virtual void Disable()
    {
        if(afterShootDelay != null) StopCoroutine(afterShootDelay);
        afterShootDelay = null;

        if (reloadCoroutine != null) StopCoroutine(reloadCoroutine);
        reloadCoroutine = null;
    }

    protected IEnumerator ReloadCooldown(float duration)
    {
        canShoot = false;
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
            else if (reserveAmmo >= magazineSize)
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
        canShoot = true;
        reloadCoroutine = null;
    }
}