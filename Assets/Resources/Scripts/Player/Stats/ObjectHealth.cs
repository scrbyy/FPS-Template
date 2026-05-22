public class ObjectHealth : ObjectStat, IDamagable
{
    public virtual void TakeDamage(float damage)
    {
        Decrease(damage);
    }
}