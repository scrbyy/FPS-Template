using UnityEngine;

public class StaminaBar : ProgressBar
{
    [SerializeField] private PlayerStamina _playerStamina;

    private void OnEnable()
    {
        _playerStamina.OnPlayerStaminaChanged += SetValue;
    }

    private void OnDisable()
    {
        _playerStamina.OnPlayerStaminaChanged -= SetValue;
    }
}
