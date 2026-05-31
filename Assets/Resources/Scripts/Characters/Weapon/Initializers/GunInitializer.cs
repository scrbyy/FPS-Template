public class GunInitializer : WeaponInitializer<Gun>
{
    public GunInitializer(IWeaponInputProvider inputProvider) : base(inputProvider) { }

    public override void Select(Gun selectableWeapon, Gun currentWeapon)
    {
        UnsubscribeFromAttack(currentWeapon);
        UnsubscribeFromReload(currentWeapon);
        UnsubscribeToStopAttack(currentWeapon);

        SubscribeToAttack(selectableWeapon);
        SubscribeToReload(selectableWeapon);
        SubscribeToStopAttack(selectableWeapon);
    }

    public override void Initialize(Gun initializableWeapon)
    {
        SubscribeToAttack(initializableWeapon);
        SubscribeToReload(initializableWeapon);
        SubscribeToStopAttack(initializableWeapon);
    }

    protected override void SubscribeToAttack(Gun weapon)
    {
        if (weapon.RecoilType == RecoilType.Automatic) _inputProvider.OnShootStarted += weapon.Attack;
        else _inputProvider.OnShootReleased += weapon.Attack;
    }

    protected override void UnsubscribeFromAttack(Gun weapon)
    {
        if (weapon.RecoilType == RecoilType.Automatic) _inputProvider.OnShootStarted -= weapon.Attack;
        else _inputProvider.OnShootReleased -= weapon.Attack;
    }

    protected void SubscribeToStopAttack(Gun weapon)
    {
        _inputProvider.OnShootReleased += weapon.StopAttack;
    }

    protected void UnsubscribeToStopAttack(Gun weapon)
    {
        _inputProvider.OnShootReleased -= weapon.StopAttack;
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