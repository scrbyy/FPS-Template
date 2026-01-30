using System;
using UnityEngine;

class PlayerHealth : MonoBehaviour
{
    public event Action OnPlayerDeath;
    public event Action<float> OnPlayerHealthChanged;

    [SerializeField] private float health;

    public void TakeDamage(float damage)
    {
        if(damage <= 0) return;

        if (health - damage <= 0)
        {
            health = 0;
            OnPlayerHealthChanged?.Invoke(health);
            InstantDie();
        }
        else
        {
            health -= damage;
            OnPlayerHealthChanged?.Invoke(health);
        }
    }

    public void InstantDie()
    {
        Debug.Log("Player has died.");
        OnPlayerDeath?.Invoke();
    }
}
