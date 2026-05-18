using Zenject;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public event System.Action<int, int> UpdateAmmoEvent;

    [SerializeField] private WeaponContainer _weaponContainer;

    [Inject] private IInputProvider _inputProvider;

    private void OnEnable()
    {
        _inputProvider.OnReloadPerformed += _weaponContainer.GetCurrent().Reload;

        _inputProvider.OnNextWeaponSelect += SetNextWeapon;
        _inputProvider.OnPreviousWeaponSelect += SetPreviousWeapon;
    }

    private void OnDisable()
    {
        _inputProvider.OnReloadPerformed -= _weaponContainer.GetCurrent().Reload;
        _weaponContainer.GetCurrent().OnEndReloadEvent -= UpdateAmmoEvent;
        _inputProvider.OnNextWeaponSelect -= SetNextWeapon;
        _inputProvider.OnPreviousWeaponSelect -= SetPreviousWeapon;
    }

    private void UnsubscribeOldShootEvents(WeaponT oldWeapon)
    {
        if (oldWeapon.GetRecoilType() == RecoilType.Single)
        {
            _inputProvider.OnShootTriggered -= oldWeapon.Shoot;
            _inputProvider.OnShootTriggered -= UpdateAmmo;
        }
        else
        {
            _inputProvider.OnShootPressed -= oldWeapon.Shoot;
            _inputProvider.OnShootPressed -= UpdateAmmo;
        }
    }

    private void Start()
    {
        _weaponContainer.GetCurrent().OnEndReloadEvent += UpdateAmmoEvent;
        SetNewShootType(_weaponContainer.GetCurrent().GetRecoilType());
        UpdateAmmo();
    }

    private void SelectNewWeapon(int newWeaponID)
    {
        WeaponT oldWeapon = _weaponContainer.GetCurrent();

        oldWeapon.Disable();
        oldWeapon.gameObject.SetActive(false);
        _weaponContainer.GetCurrent().OnEndReloadEvent -= UpdateAmmoEvent;
        oldWeapon.OnEndReloadEvent -= UpdateAmmoEvent;
        _inputProvider.OnReloadPerformed -= oldWeapon.Reload;
        UnsubscribeOldShootEvents(oldWeapon);

        _weaponContainer.SelectNew(newWeaponID);
        WeaponT newWeapon = _weaponContainer.GetCurrent();

        newWeapon.gameObject.SetActive(true);
        newWeapon.OnEndReloadEvent += UpdateAmmoEvent;
        _weaponContainer.GetCurrent().OnEndReloadEvent += UpdateAmmoEvent;
        _inputProvider.OnReloadPerformed += newWeapon.Reload;
        SetNewShootType(newWeapon.GetRecoilType());
        UpdateAmmo();
    }

    private void SetNewShootType(RecoilType newType)
    {
        WeaponT currentWeapon = _weaponContainer.GetCurrent();

        if (newType == RecoilType.Single)
        {
            _inputProvider.OnShootPressed -= currentWeapon.Shoot;
            _inputProvider.OnShootPressed -= UpdateAmmo;

            _inputProvider.OnShootTriggered += currentWeapon.Shoot;
            _inputProvider.OnShootTriggered += UpdateAmmo;
        }
        else if (newType == RecoilType.Automatic) 
        {
            _inputProvider.OnShootTriggered -= currentWeapon.Shoot;
            _inputProvider.OnShootTriggered -= UpdateAmmo;

            _inputProvider.OnShootPressed += currentWeapon.Shoot;
            _inputProvider.OnShootPressed += UpdateAmmo;
        }
    }

    private void UpdateAmmo()
    {
        UpdateAmmoEvent?.Invoke(_weaponContainer.GetCurrent().GetCurrentAmmo(), _weaponContainer.GetCurrent().GetReserveAmmo());
    }

    private void SetPreviousWeapon()
    {
        int newWeaponId;
        if (_weaponContainer.GetCurrentWeaponID() - 1 <= -1)
        {
            newWeaponId = _weaponContainer.GetSelectedWeaponCount() - 1;
        }
        else
        {
            newWeaponId = _weaponContainer.GetCurrentWeaponID() - 1;
        }
        SelectNewWeapon(newWeaponId);
    }
    private void SetNextWeapon()
    {
        int newWeaponId;
        if (_weaponContainer.GetCurrentWeaponID() + 1 >= _weaponContainer.GetSelectedWeaponCount())
        {
            newWeaponId = 0;
        }
        else
        {
            newWeaponId = _weaponContainer.GetCurrentWeaponID() + 1;
        }
        SelectNewWeapon(newWeaponId);
    }
}