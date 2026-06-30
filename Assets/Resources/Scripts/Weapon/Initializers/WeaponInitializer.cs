using System.Collections.Generic;

public abstract class WeaponInitializer<T> : IWeaponInitializer where T : Weapon
{
    protected readonly List<IWeaponFeature> _features = new List<IWeaponFeature>();

    void IWeaponInitializer.Select(Weapon weapon) => Select(weapon as T);
    void IWeaponInitializer.Unselect(Weapon weapon) => Unsubscribe(weapon as T);

    public virtual void Select(T weapon)
    {
        foreach (var feature in _features) feature.Subscribe(weapon);
    }

    public virtual void Unsubscribe(T weapon)
    {
        foreach (var feature in _features) feature.Unsubscribe(weapon);
    }
}