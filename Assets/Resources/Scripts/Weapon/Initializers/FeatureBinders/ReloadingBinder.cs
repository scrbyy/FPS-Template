public class ReloadingBinder : IFeatureBinder
{
    private readonly IWeaponInputProvider _input;
    public ReloadingBinder(IWeaponInputProvider input) => _input = input;

    public void Subscribe(Weapon weapon)
    {
        if (weapon is Gun gun) _input.OnReloadStarted += gun.Reload;
    }

    public void Unsubscribe(Weapon weapon)
    {
        if (weapon is Gun gun) _input.OnReloadStarted -= gun.Reload;
    }
}