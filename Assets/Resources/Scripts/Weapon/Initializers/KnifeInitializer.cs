using System.Collections.Generic;

namespace scrbyy.FpsTemplate.Weapons.Initializers
{
    public class KnifeInitializer : WeaponInitializer<Knife>
    {
        public KnifeInitializer(AttackBinder attackBinder) : base(new List<IFeatureBinder> { attackBinder })
        { }
    }
}