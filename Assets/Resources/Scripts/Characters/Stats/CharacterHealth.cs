using scrbyy.FpsTemplate.Characters.Damage;

namespace scrbyy.FpsTemplate.Characters.Stats
{
    public class CharacterHealth : CharacterStat, IDamageable
    {
        public virtual void TakeDamage(float damage)
        {
            Decrease(damage);
        }
    }
}