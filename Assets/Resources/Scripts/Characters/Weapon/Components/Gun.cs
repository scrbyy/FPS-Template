using System;
using UnityEngine;

public class Gun : Weapon, IShootable
{
    [SerializeField] private GunData _gunData;

    [SerializeField] private GunReloader _reloader;
    [SerializeField] private GunShooter _shooter;

    public RecoilType RecoilType => _gunData.RecoilType;

    public int CurrentAmmo => _reloader.CurrentAmmo;

    public int ReserveAmmo => _reloader.CurrentAmmo;

    public event Action<int, int> OnAmmoChanged;

    public override void Attack()
    {
        if (_reloader.IsReloading == false && _reloader.CanShoot() && _shooter.IsShooting == false)
        {
            _reloader.UseBullet();
            _shooter.Shoot();
            NotifyUpdateAmmo();
            OnAttack?.Invoke();
        }
    }

    public void Reload()
    {
        if (_shooter.IsShooting == false)
        {
            _reloader.Reload();
        }
    }

    public void Awake()
    {
        _reloader.Initialize(_gunData);
        _shooter.Initialize(_gunData);
        NotifyUpdateAmmo();
    }

    private void NotifyUpdateAmmo()
    {
        OnAmmoChanged?.Invoke(_reloader.CurrentAmmo, _reloader.ReserveAmmo);
    }

    private void OnEnable()
    {
        _reloader.OnReloadEnd += NotifyUpdateAmmo;
    }

    private void OnDisable()
    {
        _reloader.OnReloadEnd -= NotifyUpdateAmmo;
    }
}