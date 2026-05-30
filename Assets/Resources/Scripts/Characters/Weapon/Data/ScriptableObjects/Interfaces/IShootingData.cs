public interface IShootingData
{
    public float Damage { get; }

    public float Distance { get; }

    public float AfterShotDelay { get; }

    public ShootingMethod ShootingMethod { get; }

    public float DistanceModifier { get; }

    public float DamageDecreasingStep { get; }
}