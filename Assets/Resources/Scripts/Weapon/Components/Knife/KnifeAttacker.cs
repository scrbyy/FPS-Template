using UnityEngine;

using scrbyy.FpsTemplate.Weapons.AttackMethods;

namespace scrbyy.FpsTemplate.Weapons
{
    public class KnifeAttacker : WeaponAttacker
    {
        public KnifeAttacker(AttackMethodData attackConfig,
                Transform origin,
                IAttackData attackData,
                AttackMethodFactory attackFactory) : base(attackConfig, origin, attackData, attackFactory)
        { }
    }
}