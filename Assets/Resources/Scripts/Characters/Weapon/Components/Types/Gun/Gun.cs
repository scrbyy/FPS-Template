using System;
using UnityEngine;

public class Gun : Weapon, IShootable
{
    public event Action<int, int> OnAmmoChanged;

    public int CurrentAmmo => _reloader.CurrentAmmo;
    public int ReserveAmmo => _reloader.ReserveAmmo;

    public FireMode AttackMode => _data.FireMode;

    private GunReloader _reloader;

    public override void Initialize()
    {
        base.Initialize();
        if (_data.GetType() != typeof(GunData)) Debug.Log("Wrong data asset!");

        if(_weaponAttacker == null && _reloader == null)
        {
            _reloader = new GunReloader(_data as GunData);
            _weaponAttacker = new GunAttacker(_origin, _reloader.CanShoot, _data as GunData, _data);
        }

        _reloader.Initialize();
        _weaponAttacker.Initialize();

        _reloader.OnReloadEnd += NotifyUpdateAmmo;
        _weaponAttacker.OnShoot += NotifyAttack;
        _weaponAttacker.OnShotContact += NotifyContact;

        NotifyUpdateAmmo();
    }

    public override void Deinitialize()
    {
        base.Deinitialize();
        _reloader.Deinitialize();
        _weaponAttacker.Deinitialize();

        _weaponAttacker.OnShoot -= NotifyAttack;
        _weaponAttacker.OnShotContact -= NotifyContact;
        _reloader.OnReloadEnd -= NotifyUpdateAmmo;
    }

    public void Reload()
    {
        if (_isOpen == false) return;
        if (_weaponAttacker.IsAttacking == false)
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