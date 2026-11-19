using scrbyy.FpsTemplate.Characters.Player.Movement;

namespace scrbyy.FpsTemplate.Weapons
{
    public class WeaponSpeedModifier : ISpeedModifier
    {
        public float SpeedMultiplier { get; }
        public WeaponSpeedModifier(float multiplier) => SpeedMultiplier = multiplier;
    }
}