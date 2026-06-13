public class GunInitializer : WeaponInitializer<Gun>
{
    public GunInitializer(IWeaponInputProvider inputProvider) : base(inputProvider) { }

    public override void Select(Gun selectableGun)
    {
        SubscribeToAttack(selectableGun);
        SubscribeToReload(selectableGun);
        SubscribeToStopAttack(selectableGun);
        selectableGun.Initialize();
    }

    public override void Unselect(Gun gun)
    {
        UnsubscribeFromAttack(gun);
        UnsubscribeToStopAttack(gun);
        UnsubscribeFromReload(gun);

        gun.Deinitialize();
    }

    protected override void SubscribeToAttack(Gun gun)
    {
        if (gun.RecoilType == RecoilType.Automatic) _inputProvider.OnShootStarted += gun.Attack;
        else _inputProvider.OnShootReleased += gun.Attack;
    }

    protected override void UnsubscribeFromAttack(Gun gun)
    {
        if (gun.RecoilType == RecoilType.Automatic) _inputProvider.OnShootStarted -= gun.Attack;
        else _inputProvider.OnShootReleased -= gun.Attack;
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