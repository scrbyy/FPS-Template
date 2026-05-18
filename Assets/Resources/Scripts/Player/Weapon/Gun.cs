using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private GunData _gunData;

    [SerializeField] private GunReloader _reloader;
    [SerializeField] private GunShooter _shooter;

    public RecoilType RecoilType => _gunData.RecoilType;
    
    public override void Attack()
    {
        if (_reloader.IsReloading == false && _reloader.CanShoot)
        {
            _reloader.UseBullet();
            _shooter.Shoot();
        }
    }

    public void Reload()
    {
        if (_shooter.IsShooting == false)
        {
            _reloader.Reload();
        }
    }

    public void Start()
    {
        _reloader.Initialize(_gunData);
        _shooter.Initialize(_gunData);
    }
}