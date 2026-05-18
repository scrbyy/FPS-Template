public class GunInitializer : WeaponInitializer<Gun>
{
    public GunInitializer(IInputProvider inputProvider) : base(inputProvider)
    {
    }

    protected void SubscribeToReload(Gun gun)
    {
        _inputProvider.OnReloadPerformed += gun.Reload;
    }

    protected void UnsubscribeFromReload(Gun gun)
    {
        _inputProvider.OnReloadPerformed -= gun.Reload;
    }

    public override void Select(Gun selectableWeapon, Gun currentWeapon)
    {
        SubscribeToAttack(selectableWeapon);
        SubscribeToReload(selectableWeapon);
    }

    public override void Initialize(Gun initializableWeapon)
    {
        SubscribeToAttack(initializableWeapon);
        SubscribeToReload(initializableWeapon);
    }
}