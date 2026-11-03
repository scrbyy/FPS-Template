using System.Collections.Generic;

public class GunInitializer : WeaponInitializer<Gun>
{
    public GunInitializer(AttackBinder attackBinder, ReloadingBinder reloadingBinder)
        : base(new List<IFeatureBinder> { attackBinder, reloadingBinder})
    { }
}