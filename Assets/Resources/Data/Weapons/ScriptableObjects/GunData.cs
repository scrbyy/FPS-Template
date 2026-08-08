using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data Asset", menuName = "Data Assets/Weapons/Gun Data Asset")] 
public class GunData : WeaponData, IAmmoData, IStatData, IShootingData
{
    public int StartAmmo => _startAmmo;
    public int MagazineSize => _magazineSize;
    public int ReserveAmmo => _reserveAmmo;

    public float ReloadDuration => _reloadDuration;

    public float MaxDistance => _maxDistance;
    public float DistanceDamageMultiplier => _distanceDamageMultiplier;
    public float DamageDecreasingStep => _damageDecreasingStep;

    public FireMode FireMode => _fireMode;

    [Header("Distance")]
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _distanceDamageMultiplier;
    [SerializeField] private float _damageDecreasingStep;

    [Header("Ammunition")]
    [SerializeField] private int _startAmmo;
    [SerializeField] private int _magazineSize;

    [Header("Reloading")]
    [SerializeField] private int _reserveAmmo;
    [SerializeField] private float _reloadDuration;

    [Header("Shooting")]
    [SerializeField] private FireMode _fireMode;
}