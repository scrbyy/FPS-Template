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
    private int _selectedWeaponID = 0;

    [Inject] private ILoadoutInputProvider _inputProvider;

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
                weapon.gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        if (_weaponList.Count > 0)
        {
            _selectedWeapon = _weaponList[_selectedWeaponID];

            if (_initializersRegistry.TryGetValue(_selectedWeapon, out IWeaponInitializer initializer))
            {
                _selectedWeapon.gameObject.SetActive(true);
                initializer.Select(_selectedWeapon);
            }
        }
    }

    public void SwitchWeapon(Weapon newWeapon)
    {
        if (newWeapon == _selectedWeapon) return;

        if (_initializersRegistry.TryGetValue(_selectedWeapon, out IWeaponInitializer oldInitializer))
        {
            oldInitializer.Unselect(_selectedWeapon);
            _selectedWeapon.gameObject.SetActive(false);

            if (_initializersRegistry.TryGetValue(newWeapon, out IWeaponInitializer newInitializer))
            {
                _selectedWeapon = newWeapon;

                _selectedWeapon.gameObject.SetActive(true);
                newInitializer.Select(_selectedWeapon);

                OnNewWeaponSelected?.Invoke();
            }
        }
    }

    private void SetPreviousWeapon()
    {
        int newWeaponID;
        if (_selectedWeaponID - 1 <= -1)
            newWeaponID = _weaponList.Count - 1;
        else
            newWeaponID = _selectedWeaponID - 1;
        SwitchWeapon(_weaponList[newWeaponID]);
    }

    private void SetNextWeapon()
    {
        int newWeaponID;
        if (_selectedWeaponID + 1 >= _weaponList.Count)
            newWeaponID = 0;
        else
            newWeaponID = _selectedWeaponID + 1;
        SwitchWeapon(_weaponList[newWeaponID]);
    }

    private void OnEnable()
    {
        _inputProvider.OnNextWeaponSelect += SetNextWeapon;
        _inputProvider.OnPreviousWeaponSelect += SetPreviousWeapon;
    }

    private void OnDisable()
    {
        _inputProvider.OnNextWeaponSelect -= SetNextWeapon;
        _inputProvider.OnPreviousWeaponSelect -= SetPreviousWeapon;
    }
}