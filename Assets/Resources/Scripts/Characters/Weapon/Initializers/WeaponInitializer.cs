public abstract class WeaponInitializer<T> : IWeaponInitializer where T : Weapon
{
    protected readonly IWeaponInputProvider _inputProvider;

    protected WeaponInitializer(IWeaponInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    void IWeaponInitializer.Select(Weapon selectable, Weapon current)
    {
        Select(selectable as T, current as T);
    }

    void IWeaponInitializer.Initialize(Weapon initializable)
    {
        Initialize(initializable as T);
    }

    public virtual void Select(T selectableWeapon, T currentWeapon)
    {
        if (currentWeapon != null) UnsubscribeFromAttack(currentWeapon);
        if (selectableWeapon != null) SubscribeToAttack(selectableWeapon);
    }

    public virtual void Initialize(T initializableWeapon)
    {
        if (initializableWeapon != null) SubscribeToAttack(initializableWeapon);
    }

    protected virtual void SubscribeToAttack(T weapon) => _inputProvider.OnShootStarted += weapon.Attack;
    protected virtual void UnsubscribeFromAttack(T weapon) => _inputProvider.OnShootStarted -= weapon.Attack;
}