using System;
using Zenject;
using UnityEngine;
using System.Collections.Generic;

public class WeaponInventory : MonoBehaviour
{
    public event Action<Weapon> OnWeaponUnselect;

    public event Action<Weapon> OnWeaponSelected;

    public Weapon SelectedWeapon => _selectedWeapon;

    [SerializeField] private List<Weapon> _weaponList = new List<Weapon>();

    private Weapon _selectedWeapon;
    private int _selectedWeaponID = 0;

    [Inject] private ILoadoutInputProvider _inputProvider;
    [Inject] private WeaponInitializersRegistry _InitializersRegistry;


    private void Awake()
    {
        foreach (var weapon in _weaponList)
        {
            if (weapon is Gun gun)
            {
                weapon.gameObject.SetActive(false);
            }
        }

        if (_weaponList.Count > 0)
        {
            _selectedWeapon = _weaponList[_selectedWeaponID];

            if (_InitializersRegistry.TryGetInitializer(_selectedWeapon, out IWeaponInitializer initializer))
            {
                _selectedWeapon.gameObject.SetActive(true);
                _selectedWeapon.Initialize();
                initializer.Select(_selectedWeapon);
            }
        }
    }

    public void SwitchWeapon(int newWeaponID)
    {
        if (_InitializersRegistry.TryGetInitializer(_selectedWeapon, out IWeaponInitializer oldInitializer))
        {
            oldInitializer.Unselect(_selectedWeapon);
            _selectedWeapon.gameObject.SetActive(false);
            _selectedWeapon.Deinitialize();
            OnWeaponUnselect?.Invoke(_selectedWeapon);

            if (_InitializersRegistry.TryGetInitializer(_weaponList[newWeaponID], out IWeaponInitializer newInitializer))
            {
                _selectedWeapon = _weaponList[newWeaponID];

                _selectedWeapon.gameObject.SetActive(true);
                _selectedWeapon.Initialize();
                newInitializer.Select(_selectedWeapon);

                _selectedWeaponID = newWeaponID;

                OnWeaponSelected?.Invoke(_selectedWeapon);
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