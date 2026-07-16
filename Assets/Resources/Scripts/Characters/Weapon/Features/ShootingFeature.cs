public class ShootingFeature : IWeaponFeature
{
    private readonly IWeaponInputProvider _input;
    public ShootingFeature(IWeaponInputProvider input) => _input = input;

    public void Subscribe(Weapon weapon)
    {
        if (weapon is Gun gun)
        {
            if (gun.RecoilType == RecoilType.Automatic) _input.OnShootStarted += gun.Attack;
            else _input.OnShootReleased += gun.Attack;
            _input.OnShootReleased += gun.StopAttack;
        }
    }

    public void Unsubscribe(Weapon weapon)
    {
        if (weapon is Gun gun)
        {
            if (gun.RecoilType == RecoilType.Automatic) _input.OnShootStarted -= gun.Attack;
            else _input.OnShootReleased -= gun.Attack;
            _input.OnShootReleased -= gun.StopAttack;
        }
    }
}