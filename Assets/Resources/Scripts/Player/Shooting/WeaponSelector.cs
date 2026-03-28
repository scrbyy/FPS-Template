using Zenject;
using UnityEngine;
using System.Collections.Generic;

public class WeaponSelector : MonoBehaviour
{
    public event System.Action<int, int> UpdateAmmoEvent;

    [SerializeField] private Weapon[] weaponsInventory;
    [SerializeField] private List<Weapon> selectedWeapons;
    [SerializeField] private int currentWeaponIndex;

    [Inject] private IInputProvider inputProvider;

    private void Start()
    {
        InitializeWeapon(currentWeaponIndex);
    }

    private void OnEnable()
    {
        inputProvider.OnReloadPerformed += selectedWeapons[currentWeaponIndex].Reload;
        inputProvider.OnNextWeaponSelect += SetNextWeapon;
        inputProvider.OnPreviousWeaponSelect += SetPreviousWeapon;
    }

    private void OnDisable()
    {
        inputProvider.OnReloadPerformed -= selectedWeapons[currentWeaponIndex].Reload;
        inputProvider.OnNextWeaponSelect -= SetNextWeapon;
        inputProvider.OnPreviousWeaponSelect -= SetPreviousWeapon;
    }

    private void InitializeWeapon(int initializeWeaponID)
    {
        for (int i = 0; i < selectedWeapons.Count; i++)
        {
            selectedWeapons[i].gameObject.SetActive(false);
        }

        selectedWeapons[initializeWeaponID].gameObject.SetActive(true);
        selectedWeapons[initializeWeaponID].OnEndReloadEvent += UpdateAmmoEvent;
        inputProvider.OnReloadPerformed += selectedWeapons[initializeWeaponID].Reload;
        SetNewShootType(selectedWeapons[initializeWeaponID].shootType);
        UpdateAmmo();
        currentWeaponIndex = initializeWeaponID;
    }

    private void UnsubscribeOldShootEvents(Weapon oldWeapon)
    {
        if (oldWeapon.shootType == ShootType.Single)
        {
            inputProvider.OnShootTriggered -= oldWeapon.Shoot;
            inputProvider.OnShootTriggered -= UpdateAmmo;
        }
        else
        {
            inputProvider.OnShootPressed -= oldWeapon.Shoot;
            inputProvider.OnShootPressed -= UpdateAmmo;
        }
    }

    private void SelectNewWeapon(int newWeaponID)
    {
        Weapon oldWeapon = selectedWeapons[currentWeaponIndex];

        oldWeapon.OnEndReloadEvent -= UpdateAmmoEvent;
        inputProvider.OnReloadPerformed -= oldWeapon.Reload;
        oldWeapon.Disable();
        oldWeapon.gameObject.SetActive(false);

        UnsubscribeOldShootEvents(oldWeapon);

        currentWeaponIndex = newWeaponID;
        Weapon newWeapon = selectedWeapons[currentWeaponIndex];

        newWeapon.gameObject.SetActive(true);
        newWeapon.OnEndReloadEvent += UpdateAmmoEvent;
        inputProvider.OnReloadPerformed += newWeapon.Reload;
        SetNewShootType(newWeapon.shootType);
        UpdateAmmo();
    }

    private void SetNewShootType(ShootType newType)
    {
        Weapon currentWeapon = selectedWeapons[currentWeaponIndex];

        if (newType == ShootType.Single)
        {
            inputProvider.OnShootPressed -= currentWeapon.Shoot;
            inputProvider.OnShootPressed -= UpdateAmmo;

            inputProvider.OnShootTriggered += currentWeapon.Shoot;
            inputProvider.OnShootTriggered += UpdateAmmo;
        }
        else
        {
            inputProvider.OnShootTriggered -= currentWeapon.Shoot;
            inputProvider.OnShootTriggered -= UpdateAmmo;

            inputProvider.OnShootPressed += currentWeapon.Shoot;
            inputProvider.OnShootPressed += UpdateAmmo;
        }
    }

    private void UpdateAmmo()
    {
        UpdateAmmoEvent?.Invoke(selectedWeapons[currentWeaponIndex].GetCurrentAmmo(), selectedWeapons[currentWeaponIndex].GetReserveAmmo());
    }

    private void SetPreviousWeapon()
    {
        int newWeaponID;
        if (currentWeaponIndex - 1 <= -1)
            newWeaponID = selectedWeapons.Count - 1;
        else
            newWeaponID = currentWeaponIndex - 1;
        SelectNewWeapon(newWeaponID);
    }
    private void SetNextWeapon()
    {
        int newWeaponID;
        if (currentWeaponIndex + 1 >= selectedWeapons.Count)
            newWeaponID = 0;
        else
            newWeaponID = currentWeaponIndex + 1;
        SelectNewWeapon(newWeaponID);
    }
}