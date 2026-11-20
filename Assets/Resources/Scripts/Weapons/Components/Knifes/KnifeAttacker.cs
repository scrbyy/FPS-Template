using UnityEngine;

using scrbyy.FpsTemplate.Weapons.AttackMethods;
using scrbyy.FpsTemplate.Weapons.Data.Interfaces;

namespace scrbyy.FpsTemplate.Weapons.Knifes
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