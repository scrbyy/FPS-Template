using System.Collections.Generic;

using scrbyy.FpsTemplate.Weapons.Initialization;

namespace scrbyy.FpsTemplate.Weapons.Guns
{
    public class GunInitializer : WeaponInitializer<Gun>
    {
        public GunInitializer(AttackBinder attackBinder, ReloadingBinder reloadingBinder)
            : base(new List<IFeatureBinder> { attackBinder, reloadingBinder })
        { }
    }
}