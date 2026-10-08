using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private WeaponInventory _weaponInventory;

    private IAmmoHolder _currentAmmoWeapon;

    private void OnEnable()
    {
        if (_weaponInventory != null)
        {
            _weaponInventory.OnWeaponSelected += HandleWeaponSelected;
            _weaponInventory.OnWeaponUnselected += HandleWeaponUnselected;

            HandleWeaponSelected(_weaponInventory.SelectedWeapon);
        }
    }

    private void OnDisable()
    {
        if (_weaponInventory != null)
        {
            _weaponInventory.OnWeaponSelected -= HandleWeaponSelected;
            _weaponInventory.OnWeaponUnselected -= HandleWeaponUnselected;
        }

        if (_currentAmmoWeapon != null)
        {
            UnsubscribeFromAmmoEvent(_currentAmmoWeapon);
            _currentAmmoWeapon = null;
        }
    }

    private void HandleWeaponSelected(Weapon weapon)
    {
        if (weapon is IAmmoHolder ammoWeapon)
        {
            _currentAmmoWeapon = ammoWeapon;
            SubscribeToAmmoEvents(_currentAmmoWeapon);

            UpdateText(_currentAmmoWeapon.CurrentAmmo, _currentAmmoWeapon.ReserveAmmo);
        }
        else
        {
            _ammoText.text = string.Empty;
        }
    }

    private void HandleWeaponUnselected(Weapon weapon)
    {
        if (weapon is IAmmoHolder ammoWeapon && _currentAmmoWeapon == ammoWeapon)
        {
            UnsubscribeFromAmmoEvent(ammoWeapon);
            _currentAmmoWeapon = null;
            _ammoText.text = string.Empty;
        }
    }

    private void SubscribeToAmmoEvents(IAmmoHolder ammoWeapon)
    {
        ammoWeapon.OnAmmoChanged -= UpdateText; 
        ammoWeapon.OnAmmoChanged += UpdateText;
    }

    private void UnsubscribeFromAmmoEvent(IAmmoHolder ammoWeapon)
    {
        ammoWeapon.OnAmmoChanged -= UpdateText;
    }

    private void UpdateText(int currentAmmo, int reserveAmmo)
    {
        _ammoText.text = $"{currentAmmo}/{reserveAmmo}";
    }
}