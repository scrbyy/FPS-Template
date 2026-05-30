using System;
using Zenject;
using UnityEngine;
using System.Collections.Generic;

public class WeaponInventory : MonoBehaviour
{
    public event Action OnNewWeaponSelected;

    public Weapon SelectedWeapon => _selectedWeapon;

    [SerializeField] private List<Weapon> _weaponList = new List<Weapon>();

    private List<Gun> _gunList = new List<Gun>();

    private Dictionary<Weapon, IWeaponInitializer> _initializersRegistry = new Dictionary<Weapon, IWeaponInitializer>();

    private Weapon _selectedWeapon;

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

    private void Awake()
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
            OnNewWeaponSelected?.Invoke();
            initializer.Select(newWeapon, _selectedWeapon);

            _selectedWeapon.gameObject.SetActive(false);
            newWeapon.gameObject.SetActive(true);

            _selectedWeapon = newWeapon;
        }
    }
}