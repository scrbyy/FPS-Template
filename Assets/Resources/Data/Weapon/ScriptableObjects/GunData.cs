using UnityEngine;
[CreateAssetMenu(fileName = "Gun Data Asset", menuName = "Data Assets/Weapon System/Gun Data Asset")] 
public class GunData : WeaponData, IAmmoData, IStatData, IShootingData
{
    public int StartAmmo => _startAmmo;

    public int MagazineSize => _magazineSize;

    public int ReserveAmmo => _reserveAmmo;

    public float ReloadDuration => _reloadDuration;

    public float AfterShotDelay => _afterShootDelay;

    public RecoilType RecoilType => _recoilType;

    public float Distance => _distance;

    public ShootingMethod ShootingMethod => _shootingMethod;

    public float DistanceModifier => _distanceModifier;

    public float DamageDecreasingStep => _damageDecreasingStep;

    public GameObject Decal => _decal;

    [Header("Distance")]
    [SerializeField] private float _distance;
    [SerializeField] private float _distanceModifier;
    [SerializeField] private float _damageDecreasingStep;

    [Header("Ammunition")]
    [SerializeField] private int _startAmmo;
    [SerializeField] private int _magazineSize;

    [Header("Reloading")]
    [SerializeField] private int _reserveAmmo;
    [SerializeField] private float _reloadDuration;

    [Header("Shooting")]
    [SerializeField] private float _afterShootDelay;
    [SerializeField] private RecoilType _recoilType;
    [SerializeField] private ShootingMethod _shootingMethod;

    [SerializeField] private GameObject _decal;
}