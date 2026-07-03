using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Gun : Weapon, IShootable
{
    public event Action<int, int> OnAmmoChanged;

    public int CurrentAmmo => _reloader.CurrentAmmo;
    public int ReserveAmmo => _reloader.ReserveAmmo;

    public RecoilType RecoilType => _gunData.RecoilType;

    [SerializeField] private Transform _origin;
    [SerializeField] private GunData _gunData;

    private GunReloader _reloader;
    private GunShooter _shooter;

    private WeaponSpeedModifier _speedModifier;

    public override void Initialize()
    {   
        if(_shooter == null && _reloader == null)
        {
            _reloader = new GunReloader(_gunData);
            _shooter = new GunShooter(_gunData, _origin, _reloader.CanShoot);
        }

        _reloader.Initialize();
        _shooter.Initialize();

        _reloader.OnReloadEnd += NotifyUpdateAmmo;
        _shooter.OnShoot += NotifyAttack;

        _speedModifier = new WeaponSpeedModifier(_gunData.SpeedMultiplier);
        _ownerSpeedHandler.AddModifier(_speedModifier);

        NotifyUpdateAmmo();
    }

    public override void Deinitialize()
    {
        _reloader.Deinitialize();
        _shooter.Deinitialize();

        _ownerSpeedHandler.RemoveModifier(_speedModifier);
        _reloader.OnReloadEnd -= NotifyUpdateAmmo;

        _shooter.OnShoot -= NotifyAttack;
    }

    public override void Attack()
    {
        if (_reloader.CanShoot())
        {
            _shooter.StartShoot().Forget();
        }
    }

    public void StopAttack()
    {
        _shooter.StopShoot();
    }

    public void Reload()
    {
        if (_shooter.IsShooting == false)
        {
            _reloader.Reload();
        }
    }

    private void NotifyAttack()
    {
        OnAttack?.Invoke();
        _reloader.UseBullet();
        NotifyUpdateAmmo();
    }


    private void NotifyUpdateAmmo()
    {
        OnAmmoChanged?.Invoke(_reloader.CurrentAmmo, _reloader.ReserveAmmo);
    }
}