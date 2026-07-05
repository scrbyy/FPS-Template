using System;
using System.Collections.Generic;
using UnityEngine;

public class GunDecalHandler : MonoBehaviour
{
    public event Action OnDecalSpawned;

    [SerializeField] private List<WeaponInventory> _shootingCharacters;
    [SerializeField] private DecalPool _decalPool;

    private void Start()
    {
        if (_decalPool == null)
        {
            _decalPool = FindFirstObjectByType<DecalPool>();
        }

        foreach (var character in _shootingCharacters)
        {
            if (character == null) continue;

            SubscribeToGun(character.SelectedWeapon);

            character.OnWeaponSelected += HandleWeaponSelected;
            character.OnWeaponUnselect += HandleWeaponUnselected;
        }
    }

    private void OnDestroy()
    {
        foreach (var character in _shootingCharacters)
        {
            if (character != null)
            {
                UnsubscribeFromGun(character.SelectedWeapon);
                character.OnWeaponSelected -= HandleWeaponSelected;
                character.OnWeaponUnselect -= HandleWeaponUnselected;
            }
        }
    }

    private void HandleWeaponSelected(Weapon weapon)
    {
        SubscribeToGun(weapon);
    }

    private void HandleWeaponUnselected(Weapon weapon)
    {
        UnsubscribeFromGun(weapon);
    }

    private void SubscribeToGun(Weapon weapon)
    {
        if (weapon is Gun gun)
        {
            gun.OnShotContact -= CreateDecal;
            gun.OnShotContact += CreateDecal;
        }
    }

    private void UnsubscribeFromGun(Weapon weapon)
    {
        if (weapon is Gun gun)
        {
            gun.OnShotContact -= CreateDecal;
        }
    }

    private void CreateDecal(HitData hitData)
    {
        if (_decalPool != null)
        {
            _decalPool.Get(hitData);
            OnDecalSpawned?.Invoke();
        }
    }
}