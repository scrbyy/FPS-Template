using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CharacterHittablePart : MonoBehaviour, IDamageable
{
    [SerializeField] private CharacterHealth _attachedHealth;
    [SerializeField, Min(0f)] private float _damageMultiplier;

    public void TakeDamage(float damage)
    {
        _attachedHealth.TakeDamage(damage * _damageMultiplier);
    }
}