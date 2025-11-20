using UnityEngine;
using System.Collections.Generic;

public class WeaponSelector : MonoBehaviour
{
    public event System.Action<int, int> UpdateAmmoEvent;

    [SerializeField] private Weapon[] weaponsInventory;
    [SerializeField] private List<Weapon> selectedWeapons;
    [SerializeField] private InputProvider inputProvider;
    private int currentWeaponIndex;
    
    private void Start()
    {
        currentWeaponIndex = 0;

         UpdateAmmoEvent?.Invoke(selectedWeapons[currentWeaponIndex].GetCurrentAmmo(), selectedWeapons[currentWeaponIndex].GetReserveAmmo());

        selectedWeapons[currentWeaponIndex].EndReloadEvent += UpdateAmmoEvent;

        SetNewShootType(selectedWeapons[currentWeaponIndex].shootType);
    }

    private void OnEnable()
    {
        inputProvider.OnReloadPerformed += selectedWeapons[currentWeaponIndex].Reload;
    }

    private void OnDisable()
    {
        inputProvider.OnReloadPerformed -= selectedWeapons[currentWeaponIndex].Reload;
    }

    private void SelectNewWeapon(int newWeaponID)
    {
        selectedWeapons[currentWeaponIndex].EndReloadEvent -= UpdateAmmoEvent;

        inputProvider.OnReloadPerformed -= selectedWeapons[currentWeaponIndex].Reload;

        currentWeaponIndex = newWeaponID;

        selectedWeapons[newWeaponID].EndReloadEvent += UpdateAmmoEvent;

        UpdateAmmoEvent?.Invoke(selectedWeapons[newWeaponID].GetCurrentAmmo(), selectedWeapons[newWeaponID].GetReserveAmmo());

        inputProvider.OnReloadPerformed += selectedWeapons[newWeaponID].Reload;

        SetNewShootType(selectedWeapons[newWeaponID].shootType);
    }

    private void SetNewShootType(ShootType newType)
    {
        if(newType != ShootType.Automatic)
        {
            inputProvider.OnShootPressed -= selectedWeapons[currentWeaponIndex].Shoot;
            inputProvider.OnShootPressed -= UpdateAmmo;

            inputProvider.OnShootTriggered += selectedWeapons[currentWeaponIndex].Shoot;
            inputProvider.OnShootTriggered += UpdateAmmo;
        }
        else
        {
            inputProvider.OnShootTriggered -= selectedWeapons[currentWeaponIndex].Shoot;
            inputProvider.OnShootTriggered -= UpdateAmmo;

            inputProvider.OnShootPressed += selectedWeapons[currentWeaponIndex].Shoot;
            inputProvider.OnShootPressed += UpdateAmmo;
        }
    }

    private void UpdateAmmo()
    {
        UpdateAmmoEvent?.Invoke(selectedWeapons[currentWeaponIndex].GetCurrentAmmo(), selectedWeapons[currentWeaponIndex].GetReserveAmmo());
    }
}