using TMPro;
using UnityEngine;
public class AmmoUI : MonoBehaviour 
{
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private WeaponSelector weaponInventory;

    public void UpdateAllAmmo(int currentAmmo, int reserveAmmo)
    {
        ammoText.text = $"{currentAmmo} / {reserveAmmo}";
    }

    private void OnEnable()
    {
        weaponInventory. UpdateAmmoEvent += UpdateAllAmmo;
    }

    private void OnDisable()
    {
        weaponInventory.UpdateAmmoEvent -= UpdateAllAmmo;
    }
}

