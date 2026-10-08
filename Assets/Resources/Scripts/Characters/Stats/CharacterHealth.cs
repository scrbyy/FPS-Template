public class CharacterHealth : CharacterStat, IDamageable
{
    public virtual void TakeDamage(int damage)
    {
        Decrease(damage);
    }
}