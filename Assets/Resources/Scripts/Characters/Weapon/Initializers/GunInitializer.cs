public class GunInitializer : WeaponInitializer<Gun>
{
    public GunInitializer(IWeaponInputProvider inputProvider) : base(inputProvider) { }

    public override void Select(Gun selectableWeapon, Gun currentWeapon)
    {
        UnsubscribeFromAttack(currentWeapon);
        UnsubscribeFromReload(currentWeapon);

        SubscribeToAttack(selectableWeapon);
        SubscribeToReload(selectableWeapon);
    }

    public override void Initialize(Gun initializableWeapon)
    {
        SubscribeToAttack(initializableWeapon);
        SubscribeToReload(initializableWeapon);
    }

    protected override void SubscribeToAttack(Gun weapon)
    {
        if (weapon.RecoilType == RecoilType.Automatic) _inputProvider.OnShootPressed += weapon.Attack;
        else _inputProvider.OnShootStarted += weapon.Attack;
    }

    protected override void UnsubscribeFromAttack(Gun weapon)
    {
        if (weapon.RecoilType == RecoilType.Automatic) _inputProvider.OnShootPressed -= weapon.Attack;
        else _inputProvider.OnShootStarted -= weapon.Attack;
    }

    protected void SubscribeToReload(Gun gun)
    {
        _inputProvider.OnReloadStarted += gun.Reload;
    }

    protected void UnsubscribeFromReload(Gun gun)
    {
        _inputProvider.OnReloadStarted -= gun.Reload;
    }
}