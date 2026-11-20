using System.Collections.Generic;

using scrbyy.FpsTemplate.Weapons.Initialization;

namespace scrbyy.FpsTemplate.Weapons.Knifes
{
    public class KnifeInitializer : WeaponInitializer<Knife>
    {
        public KnifeInitializer(AttackBinder attackBinder) : base(new List<IFeatureBinder> { attackBinder })
        { }
    }
}