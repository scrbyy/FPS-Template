using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "FPS Template/Data Assets/Weapons/Gun Data Asset")] 
public class GunData : WeaponData, IAmmoData, IStatData, IDistanceAttackData
{
    public int StartAmmo => _startAmmo;
    public int MagazineSize => _magazineSize;
    public int ReserveAmmo => _reserveAmmo;

    public float ReloadDuration => _reloadDuration;

    public float DistanceDamageMultiplier => _distanceDamageMultiplier;
    public float DamageDecreasingStep => _damageDecreasingStep;

    [Header("Distance")]
    [SerializeField] private float _distanceDamageMultiplier;
    [SerializeField] private float _damageDecreasingStep;

    [Header("Ammunition")]
    [SerializeField] private int _startAmmo;
    [SerializeField] private int _reserveAmmo;

    [Space]
    [SerializeField] private int _magazineSize;

    [Header("Reloading")]
    [SerializeField] private float _reloadDuration;
}