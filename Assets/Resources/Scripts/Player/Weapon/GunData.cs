using UnityEngine;

public class GunData : WeaponData, IAmmoData, IStatData, IRaycastShootingData
{
    public int CurrentAmmo => _currentAmmo;

    public int MagazineSize => _bulletsInMagazine;

    public int ReserveAmmo => _reserveAmmo;

    public float ReloadDuration => _reloadDuration;

    public float AfterShotDelay => _afterShootDelay;

    public RecoilType RecoilType => _recoilType;

    public Transform Origin => _origin;

    public float Distance => _distance;

    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _bulletsInMagazine;
    [SerializeField] private int _reserveAmmo;
    [SerializeField] private float _reloadDuration;

    [SerializeField] private float _afterShootDelay;
    [SerializeField] private RecoilType _recoilType;

    [SerializeField] private Transform _origin;
    [SerializeField] private float _distance;
}