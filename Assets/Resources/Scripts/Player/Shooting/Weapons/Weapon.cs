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

    protected RaycastHit hit;

    public abstract void Shoot();

    public abstract void Reload();

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetReserveAmmo()
    {
        return reserveAmmo;
    }
}
