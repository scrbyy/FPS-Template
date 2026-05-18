using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private GunReloader _reloader;
    [SerializeField] private GunShooter _shooter;
    
    public override void Attack()
    {
        if(_reloader.IsReloading == false)
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
}