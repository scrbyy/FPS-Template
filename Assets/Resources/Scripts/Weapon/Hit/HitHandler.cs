using scrbyy.FpsTemplate.Characters.Damage;

namespace scrbyy.FpsTemplate.Weapons.Hit
{
    public class HitHandler
    {
        public void HandleHit(HitData hitData, float damage)
        {
            if (hitData.GameObject.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(damage);
            }
        }
    }
}