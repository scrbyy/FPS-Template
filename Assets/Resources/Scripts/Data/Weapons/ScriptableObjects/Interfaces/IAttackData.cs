public interface IAttackData
{
    public float Damage { get; }

    public float MaxDistance { get; }

    public float AfterAttackDelay { get; }

    public AttackType AttackMethod { get; }
}