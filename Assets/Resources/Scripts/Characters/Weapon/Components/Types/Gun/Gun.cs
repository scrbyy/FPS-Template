using System;
using UnityEngine;
using Zenject;

public class Gun : Weapon, IShootable
{
    public event Action<int, int> OnAmmoChanged;

    public int CurrentAmmo => _reloader.CurrentAmmo;
    public int ReserveAmmo => _reloader.ReserveAmmo;

    public FireMode AttackMode => _data.FireMode;

    private GunReloader _reloader;

    [Inject] private AttackMethodFactory _attackMethodFactory;

    public override void Initialize()
    {
        base.Initialize();
        if (!(_data is GunData gunData))
        {
            Debug.LogError("Wrong data asset!");
            return;
        }

        if (_weaponAttacker == null && _reloader == null)
        {
            _reloader = new GunReloader(gunData);

            _weaponAttacker = new GunAttacker(_origin, _reloader.CanShoot, gunData, gunData, _attackMethodFactory);
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