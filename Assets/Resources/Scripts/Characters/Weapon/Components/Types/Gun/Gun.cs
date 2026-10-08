using System;
using UnityEngine;
using Zenject;

public class Gun : Weapon, IAmmoHolder
{
    public event Action OnReloadStart;
    public event Action OnReady;
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
        _weaponAttacker.OnShoot += NotifyShoot;
        _weaponAttacker.OnShotContact += NotifyContact;
        _reloader.OnReloadStart += NotifyReloadStart;

        OnReady?.Invoke();
        NotifyUpdateAmmo();
    }

    public override void Deinitialize()
    {
        base.Deinitialize();

        if (_reloader != null)
        {
            _reloader.Deinitialize();
            _reloader.OnReloadEnd -= NotifyUpdateAmmo;
            _reloader.OnReloadStart -= NotifyReloadStart;
        }

        if (_weaponAttacker != null)
        {
            _weaponAttacker.Deinitialize();
            _weaponAttacker.OnShoot -= NotifyShoot;
            _weaponAttacker.OnShotContact -= NotifyContact;
        }
    }

    public void Reload()
    {
        if (!_isOpen) return;
        if (!_weaponAttacker.IsAttacking)
        {
            _reloader.Reload();
        }
    }

    private void NotifyShoot()
    {
        NotifyAttack();
        _reloader.UseBullet();
        NotifyUpdateAmmo();
    }

    private void NotifyContact(HitData hit)
    {
        NotifyShotContact(hit);
    }

    private void NotifyUpdateAmmo()
    {
        OnAmmoChanged?.Invoke(_reloader.CurrentAmmo, _reloader.ReserveAmmo);
    }

    private void NotifyReloadStart()
    {
        OnReloadStart?.Invoke();
    }
}