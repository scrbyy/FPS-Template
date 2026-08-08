using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Gun : Weapon, IShootable
{
    public event Action<int, int> OnAmmoChanged;

    public int CurrentAmmo => _reloader.CurrentAmmo;
    public int ReserveAmmo => _reloader.ReserveAmmo;

    public FireMode AttackMode => _data.FireMode;

    private GunReloader _reloader;
    private GunAttacker _shooter;

    public override void Initialize()
    {
        base.Initialize();
        if (_data.GetType() != typeof(GunData)) Debug.Log("Wrong data asset!");

        if(_shooter == null && _reloader == null)
        {
            _reloader = new GunReloader(_data as GunData);
            _shooter = new GunAttacker(_origin, _reloader.CanShoot, _data as GunData, _data);
        }

        _reloader.Initialize();
        _shooter.Initialize();

        _reloader.OnReloadEnd += NotifyUpdateAmmo;
        _shooter.OnShoot += NotifyAttack;
        _shooter.OnShotContact += NotifyContact;

        NotifyUpdateAmmo();
    }

    public override void Deinitialize()
    {
        base.Deinitialize();
        _reloader.Deinitialize();
        _shooter.Deinitialize();

        _shooter.OnShoot -= NotifyAttack;
        _shooter.OnShotContact -= NotifyContact;
        _reloader.OnReloadEnd -= NotifyUpdateAmmo;
    }

    public override void Attack()
    {
        if (_isOpen == false) return;
        _shooter.StartShoot().Forget();
    }

    public override void StopAttack()
    {
        _shooter.StopShoot();
        OnStopAttack?.Invoke();
    }

    public void Reload()
    {
        if (_isOpen == false) return;
        if (_shooter.IsAttacking == false)
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