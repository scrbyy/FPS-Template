using System.Collections.Generic;

public class KnifeInitializer : WeaponInitializer<Knife>
{
    public KnifeInitializer(AttackFeature shooting) : base(new List<IWeaponFeature> { shooting }) { 
    }
}