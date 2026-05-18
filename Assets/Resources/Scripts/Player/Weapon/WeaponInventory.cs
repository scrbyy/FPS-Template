using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weaponList = new List<Weapon>();
    private List<Gun> _gunList = new List<Gun>();

    private Weapon _selectedWeapon;
    private Dictionary<Weapon, IWeaponInitializer> _initializersRegistry = new Dictionary<Weapon, IWeaponInitializer>();

    [Inject]
    private void Construct(GunInitializer gunInitializer)
    {
        _gunList.Clear();
        _initializersRegistry.Clear();

        foreach (var weapon in _weaponList)
        {
            if (weapon is Gun gun)
            {
                _gunList.Add(gun);
                _initializersRegistry.Add(weapon, gunInitializer);
            }
        }
    }

    private void Start()
    {
        if (_weaponList.Count > 0)
        {
            _selectedWeapon = _weaponList[0];

            if (_initializersRegistry.TryGetValue(_selectedWeapon, out IWeaponInitializer initializer))
            {
                initializer.Initialize(_selectedWeapon);
            }
        }
    }

    public void SwitchWeapon(Weapon newWeapon)
    {
        if (_initializersRegistry.TryGetValue(newWeapon, out IWeaponInitializer initializer))
        {
            initializer.Select(newWeapon, _selectedWeapon);

            _selectedWeapon.gameObject.SetActive(false);
            newWeapon.gameObject.SetActive(true);

            _selectedWeapon = newWeapon;
        }
    }
}