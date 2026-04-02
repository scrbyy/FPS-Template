using UnityEngine;
using System.Collections.Generic;

public class WeaponContainer : MonoBehaviour 
{
    [SerializeField] private Weapon[] _weaponsInventory;
    [SerializeField] private List<Weapon> _selectedWeapons;

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

    public Weapon GetCurrent()
    {
        return _selectedWeapons[_currentWeaponID];
    }

    private void Start()
    {
        foreach (Weapon weapon in _weaponsInventory)
        {
            InitializeWeapon(weapon);
        }
        _selectedWeapons[_currentWeaponID].gameObject.SetActive(true);
    }

    private void InitializeWeapon(Weapon initializingWeapon)
    {
        initializingWeapon.SetShootingMethod(new RaycastShoot(initializingWeapon.GetShootOrigin(), initializingWeapon.GetShootDistance()));
        initializingWeapon.gameObject.SetActive(false);
    }
}
