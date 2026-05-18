using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour 
{
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private WeaponInventory _weaponInventory;

    private IShootable _currentAmmoWeapon;

    private void UpdateWeaponEvent()
    {
        if (_weaponInventory.SelectedWeapon is IShootable ammoWeapon)
        {
            if (_currentAmmoWeapon != null) UnsubscribeFromAmmoEvent(_currentAmmoWeapon);
            _currentAmmoWeapon = ammoWeapon;
            SubscribeToAmmoEvents(_currentAmmoWeapon);
        }
        else
        {
            if(_currentAmmoWeapon != null ) 
            UnsubscribeFromAmmoEvent(_currentAmmoWeapon);
            _ammoText.text = string.Empty;
            _currentAmmoWeapon = null;
        }
    }

    private void SubscribeToAmmoEvents(IShootable ammoWeapon)
    {
        ammoWeapon.OnAmmoChanged += UpdateText;
    }

    private void UnsubscribeFromAmmoEvent(IShootable ammoWeapon)
    {
        ammoWeapon.OnAmmoChanged -= UpdateText;
    }

    private void UpdateText(int currentAmmo, int reserveAmmo)
    {
        _ammoText.text = currentAmmo + "/" + reserveAmmo;
    }

    private void OnEnable()
    {
        UpdateWeaponEvent();
        _weaponInventory.OnNewWeaponSelected += UpdateWeaponEvent;
    }

    private void OnDisable()
    {
        _weaponInventory.OnNewWeaponSelected -= UpdateWeaponEvent;
    }
}