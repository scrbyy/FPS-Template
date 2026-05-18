using UnityEngine;
using System.Collections.Generic;

public class WeaponContainer : MonoBehaviour 
{
    [SerializeField] private WeaponT[] _weaponsInventory;
    [SerializeField] private List<WeaponT> _selectedWeapons;

    [SerializeField] private int _currentWeaponID;

    public void SelectNew(int weaponID)
    {
        _currentWeaponID = weaponID;
    }
    
    public int GetCurrentWeaponID()
    {
        return _currentWeaponID;
    }

    public int GetSelectedWeaponCount()
    {
        return _selectedWeapons.Count;
    }

    public WeaponT GetCurrent()
    {
        return _selectedWeapons[_currentWeaponID];
    }

    private void Start()
    {
        foreach (WeaponT weapon in _weaponsInventory)
        {
            InitializeWeapon(weapon);
        }
        _selectedWeapons[_currentWeaponID].gameObject.SetActive(true);
    }

    private void InitializeWeapon(WeaponT initializingWeapon)
    {
        initializingWeapon.SetShootingMethod(new RaycastShoot(initializingWeapon.GetShootOrigin(), initializingWeapon.GetShootDistance()));
        initializingWeapon.gameObject.SetActive(false);
    }
}
