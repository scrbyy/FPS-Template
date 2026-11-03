using System.Collections.Generic;

public class KnifeInitializer : WeaponInitializer<Knife>
{
    public KnifeInitializer(AttackFeature attackFeature) : base(new List<IWeaponFeature> { attackFeature }) { 
    }
}