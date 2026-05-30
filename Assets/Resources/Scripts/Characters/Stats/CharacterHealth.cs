public class CharacterHealth : CharacterStat, IDamagable
{
    public virtual void TakeDamage(float damage)
    {
        Decrease(damage);
    }
}