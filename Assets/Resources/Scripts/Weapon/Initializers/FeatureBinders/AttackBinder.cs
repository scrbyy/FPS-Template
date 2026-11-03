public class AttackBinder : IFeatureBinder
{
    private readonly IWeaponInputProvider _inputProvider;
    public AttackBinder(IWeaponInputProvider inputProvider) => _inputProvider = inputProvider;

    public void Subscribe(Weapon weapon)
    {
        if (weapon.FireMode == FireMode.Automatic) _inputProvider.OnShootStarted += weapon.Attack;
        else _inputProvider.OnShootReleased += weapon.Attack;
        _inputProvider.OnShootReleased += weapon.StopAttack;

    }

    public void Unsubscribe(Weapon weapon)
    {
        if (weapon.FireMode == FireMode.Automatic) _inputProvider.OnShootStarted -= weapon.Attack;
        else _inputProvider.OnShootReleased -= weapon.Attack;
        _inputProvider.OnShootReleased -= weapon.StopAttack;
    }
}