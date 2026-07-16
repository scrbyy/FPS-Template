public class WeaponSpeedModifier : ISpeedModifier
{
    public float SpeedMultiplier { get; }
    public WeaponSpeedModifier(float multiplier) => SpeedMultiplier = multiplier;
}