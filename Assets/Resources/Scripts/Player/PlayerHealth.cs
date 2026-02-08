using System;
using UnityEngine;

class PlayerHealth : MonoBehaviour
{
    public event Action OnPlayerDeath;
    public event Action<float> OnPlayerHealthChanged;

    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;

    public void TakeDamage(float damage)
    {
        if(damage <= 0) return;

        if (_currentHealth - damage <= 0)
        {
            _currentHealth = 0;
            OnPlayerHealthChanged?.Invoke(_currentHealth);
            InstantDie();
        }
        else
        {
            _currentHealth -= damage;
            OnPlayerHealthChanged?.Invoke(_currentHealth);
        }
    }

    public void InstantDie()
    {
        Debug.Log("Player has died.");
        OnPlayerDeath?.Invoke();
    }
    public float GetMaxHealth()
    {
        return _maxHealth;
    }
}
