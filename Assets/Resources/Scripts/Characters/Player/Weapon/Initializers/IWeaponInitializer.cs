public interface IWeaponInitializer
{
    public void Initialize(Weapon initializableWeapon);
    public void Select(Weapon selectableWeapon, Weapon currentWeapon);
}