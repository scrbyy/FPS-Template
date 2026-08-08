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
            OnWeaponUnselect?.Invoke(_selectedWeapon);
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
        // Если вышли за 0, берем последний индекс, иначе уменьшаем на 1
        int newWeaponID = (_selectedWeaponID - 1 < 0) ? _weaponList.Count - 1 : _selectedWeaponID - 1;
        SwitchWeapon(newWeaponID);
    }

    private void SetNextWeapon()
    {
        // Магическая формула остатка от деления автоматически сбросит индекс в 0 при достижении Count
        int newWeaponID = (_selectedWeaponID + 1) % _weaponList.Count;
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