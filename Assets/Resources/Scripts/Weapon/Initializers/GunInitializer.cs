public class GunInitializer : WeaponInitializer<Gun>
{
    public GunInitializer(IWeaponInputProvider inputProvider)
    {
        _features.Add(new ShootingFeature(inputProvider));
        _features.Add(new ReloadingFeature(inputProvider));
    }
}