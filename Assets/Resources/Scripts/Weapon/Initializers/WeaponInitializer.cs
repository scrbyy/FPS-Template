public abstract class WeaponInitializer<T> : IWeaponInitializer where T : Weapon
{
    protected readonly IWeaponInputProvider _inputProvider;

    protected WeaponInitializer(IWeaponInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    void IWeaponInitializer.Select(Weapon selectableWeapon)
    {
        Select(selectableWeapon as T);
    }

    void IWeaponInitializer.Unselect(Weapon unselectableWeapon)
    {
        if (unselectableWeapon != null) UnsubscribeFromAttack(unselectableWeapon as T);
    }

    public virtual void Unselect(T selectableWeapon)
    {
        Unselect(selectableWeapon);
    }

    public virtual void Select(T selectableWeapon)
    {
        if (selectableWeapon != null) SubscribeToAttack(selectableWeapon);
    }
    
    protected virtual void SubscribeToAttack(T weapon) => _inputProvider.OnShootReleased += weapon.Attack;

    protected virtual void UnsubscribeFromAttack(T weapon) => _inputProvider.OnShootReleased -= weapon.Attack;
}