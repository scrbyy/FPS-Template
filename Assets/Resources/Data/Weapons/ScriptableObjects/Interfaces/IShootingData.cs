public interface IShootingData
{
    public float Damage { get; }

    public float MaxDistance { get; }

    public float AfterAttackDelay { get; }

    public AttackMethod AttackMethod { get; }

    public float DistanceDamageMultiplier { get; }

    public float DamageDecreasingStep { get; }
}