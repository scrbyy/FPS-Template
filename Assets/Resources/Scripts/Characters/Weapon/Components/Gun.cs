using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

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
        _isOpen = false;
        _openCts = new CancellationTokenSource();

        if(_shooter == null && _reloader == null)
        {
            _reloader = new GunReloader(_gunData);
            _shooter = new GunShooter(_gunData, _origin, _reloader.CanShoot);
        }

        OpenDelay(_gunData.OpenTime).Forget();

        _reloader.Initialize();
        _shooter.Initialize();

        _reloader.OnReloadEnd += NotifyUpdateAmmo;
        _shooter.OnShoot += NotifyAttack;
        _shooter.OnShotContact += NotifyContact;

        _speedModifier = new WeaponSpeedModifier(_gunData.SpeedMultiplier);
        _ownerSpeedHandler.AddModifier(_speedModifier);

        NotifyUpdateAmmo();
    }

    public override void Deinitialize()
    {
        _reloader.Deinitialize();
        _shooter.Deinitialize();

        _shooter.OnShoot -= NotifyAttack;
        _shooter.OnShotContact -= NotifyContact;
        _reloader.OnReloadEnd -= NotifyUpdateAmmo;

        if (_openCts != null)
        {
            _openCts.Cancel();
            _openCts.Dispose();
            _openCts = null;
        }

        _isOpen = false;

        _reloader.OnReloadEnd -= NotifyUpdateAmmo;
    }


    public override void Attack()
    {
        if (_isOpen == false) return;
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
        if (_isOpen == false) return;
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
    private void NotifyContact(HitData hit)
    {
        OnShotContact?.Invoke(hit);
    }

    private void NotifyUpdateAmmo()
    {
        OnAmmoChanged?.Invoke(_reloader.CurrentAmmo, _reloader.ReserveAmmo);
    }
}