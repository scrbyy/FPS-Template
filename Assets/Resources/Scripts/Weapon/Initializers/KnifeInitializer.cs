using System.Collections.Generic;

public class KnifeInitializer : WeaponInitializer<Knife>
{
    public KnifeInitializer(AttackBinder attackBinder) : base(new List<IFeatureBinder> { attackBinder }) 
    { }
}