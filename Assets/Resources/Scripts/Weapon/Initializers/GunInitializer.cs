using System.Collections.Generic;

namespace scrbyy.FpsTemplate.Weapons.Initializers
{
    public class GunInitializer : WeaponInitializer<Gun>
    {
        public GunInitializer(AttackBinder attackBinder, ReloadingBinder reloadingBinder)
            : base(new List<IFeatureBinder> { attackBinder, reloadingBinder })
        { }
    }
}