using UnityEngine;
using Zenject;

namespace scrbyy.FpsTemplate.Weapons.AttackMethods
{
    public abstract class AttackMethodData
    {
        public abstract AttackType Type { get; }

        public abstract IAttackMethod CreateMethod(Transform origin, DiContainer container);
    }
}