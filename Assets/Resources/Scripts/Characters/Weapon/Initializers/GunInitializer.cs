using System.Collections.Generic;

public class GunInitializer : WeaponInitializer<Gun>
{
    public GunInitializer(ShootingFeature shooting, ReloadingFeature reloading)
        : base(new List<IWeaponFeature> { shooting, reloading })
    {
    }
}