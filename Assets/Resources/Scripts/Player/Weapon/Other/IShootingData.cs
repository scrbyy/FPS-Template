public interface IShootingData
{
    public float Distance { get; }

    public float AfterShotDelay { get; }

    public ShootingMethod ShootingMethod { get; }
}