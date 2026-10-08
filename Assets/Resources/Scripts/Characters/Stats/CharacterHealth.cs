public class CharacterHealth : CharacterStat, IDamageable
{
    public virtual void TakeDamage(float damage)
    {
        Decrease(damage);
    }
}