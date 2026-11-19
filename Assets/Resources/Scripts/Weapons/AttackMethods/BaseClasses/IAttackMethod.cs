using scrbyy.FpsTemplate.Weapons.Hit;

namespace scrbyy.FpsTemplate.Weapons.AttackMethods
{
    public interface IAttackMethod
    {
        public HitData Execute();
    }
}