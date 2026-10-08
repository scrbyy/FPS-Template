using System;
using Zenject;
using UnityEngine;
using System.Collections.Generic;

public class WeaponInventory : MonoBehaviour
{
    public event Action<Weapon> OnWeaponUnselected;

    public event Action<Weapon> OnWeaponSelected;

    public Weapon SelectedWeapon => _selectedWeapon;

    [SerializeField] private List<Weapon> _weaponList = new List<Weapon>();

    private Weapon _selectedWeapon;
    [SerializeField] private int _selectedWeaponID = 0;

    [Inject] private ILoadoutInputProvider _inputProvider;
    [Inject] private WeaponInitializersRegistry _initializersRegistry;


    private void Start()
    {
        foreach (var weapon in _weaponList)
        {
            weapon.gameObject.SetActive(false);
        }

        if (_weaponList.Count > 0)
        {
            SelectWeaponInternal(_selectedWeaponID);
        }
    }

    public void SwitchWeapon(int newWeaponID)
    {
        if (newWeaponID < 0 || newWeaponID >= _weaponList.Count) return;

        if (_selectedWeapon != null && _initializersRegistry.TryGetInitializer(_selectedWeapon, out var oldInitializer))
        {
            oldInitializer.Unselect(_selectedWeapon);
            _selectedWeapon.gameObject.SetActive(false);
            _selectedWeapon.Deinitialize();
            OnWeaponUnselected?.Invoke(_selectedWeapon);
        }

        SelectWeaponInternal(newWeaponID);
    }

    private void SelectWeaponInternal(int id)
    {
        _selectedWeaponID = id;
        _selectedWeapon = _weaponList[_selectedWeaponID];

        if (_initializersRegistry.TryGetInitializer(_selectedWeapon, out var newInitializer))
        {
            _selectedWeapon.gameObject.SetActive(true);
            _selectedWeapon.Initialize();
            newInitializer.Select(_selectedWeapon);
            OnWeaponSelected?.Invoke(_selectedWeapon);
        }
    }

    private void SetPreviousWeapon()
    {
        int newWeaponID = (_selectedWeaponID - 1 < 0) ? _weaponList.Count - 1 : _selectedWeaponID - 1;
        SwitchWeapon(newWeaponID);
    }

    private void SetNextWeapon()
    {
        int newWeaponID = (_selectedWeaponID + 1) % _weaponList.Count;
        SwitchWeapon(newWeaponID);
    }

    private void OnEnable()
    {
        _inputProvider.OnNextWeaponSelected += SetNextWeapon;
        _inputProvider.OnPreviousWeaponSelected += SetPreviousWeapon;
    }

    private void OnDisable()
    {
        _inputProvider.OnNextWeaponSelected -= SetNextWeapon;
        _inputProvider.OnPreviousWeaponSelected -= SetPreviousWeapon;
    }
}