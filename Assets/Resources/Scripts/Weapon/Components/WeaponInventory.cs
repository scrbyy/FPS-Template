using System;
using Zenject;
using UnityEngine;
using System.Collections.Generic;

public class WeaponInventory : MonoBehaviour
{
    public event Action OnNewWeaponSelected;

    public Weapon SelectedWeapon => _selectedWeapon;


    [SerializeField] private List<Weapon> _weaponList = new List<Weapon>();

    private Dictionary<Weapon, IWeaponInitializer> _initializersRegistry = new Dictionary<Weapon, IWeaponInitializer>();

    private Weapon _selectedWeapon;
    private int _selectedWeaponID = 0;

    [Inject] private ILoadoutInputProvider _inputProvider;

    [Inject]
    private void Construct(GunInitializer gunInitializer)
    {
        _initializersRegistry.Clear();

        foreach (var weapon in _weaponList)
        {
            if (weapon is Gun gun)
            {
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

    public void SwitchWeapon(int newWeaponID)
    {
        if (_initializersRegistry.TryGetValue(_selectedWeapon, out IWeaponInitializer oldInitializer))
        {
            oldInitializer.Unselect(_selectedWeapon);
            _selectedWeapon.gameObject.SetActive(false);

            if (_initializersRegistry.TryGetValue(_weaponList[newWeaponID], out IWeaponInitializer newInitializer))
            {
                _selectedWeapon = _weaponList[newWeaponID];

                _selectedWeapon.gameObject.SetActive(true);
                newInitializer.Select(_selectedWeapon);
                _selectedWeaponID = newWeaponID;

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
        SwitchWeapon(newWeaponID);
    }

    private void SetNextWeapon()
    {
        int newWeaponID;
        if (_selectedWeaponID + 1 >= _weaponList.Count)
        {
            newWeaponID = 0;
        }
        else
        {
            newWeaponID = _selectedWeaponID + 1;
        }
        SwitchWeapon(newWeaponID);
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