using scrbyy.FpsTemplate.Weapons.AttackMethods;

namespace scrbyy.FpsTemplate.Weapons.Data.Interfaces
{
    public interface IAttackData
    {
        public float Damage { get; }

        public float AfterAttackDelay { get; }

        public AttackType AttackMethod { get; }
    }
}