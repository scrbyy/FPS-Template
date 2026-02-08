using UnityEngine;

public class HealthBar : ProgressBar
{
    [SerializeField] private PlayerHealth _playerHealth;

    private void OnEnable()
    {
        _playerHealth.OnPlayerHealthChanged += SetValue;
    }

    private void OnDisable()
    {
        _playerHealth.OnPlayerHealthChanged -= SetValue;
    }

    protected override float GetMaxValue()
    {
       return _playerHealth.GetMaxHealth();
    }
}
