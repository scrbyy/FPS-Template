public class CharacterHealth : CharacterStat
{
    public virtual void TakeDamage(int damage)
    {
        Decrease(damage);
    }
}