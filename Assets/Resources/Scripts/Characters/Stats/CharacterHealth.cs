public class CharacterHealth : CharacterStat, IDamagable
{
    public virtual void TakeDamage(int damage)
    {
        Decrease(damage);
    }
}