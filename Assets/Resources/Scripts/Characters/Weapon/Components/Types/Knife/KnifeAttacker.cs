using UnityEngine;

public class KnifeAttacker : WeaponAttacker
{
    public KnifeAttacker(AttackData attackConfig,
            Transform origin,
            IAttackData attackData,
            AttackMethodFactory attackFactory) : base(attackConfig, origin, attackData, attackFactory)
    {
    }
}