using System.Collections.Generic;

public class GunInitializer : WeaponInitializer<Gun>
{
    public GunInitializer(AttackFeature attackFeature, ReloadingFeature reloadingFeature)
        : base(new List<IWeaponFeature> { attackFeature, reloadingFeature })
    {
    }
}