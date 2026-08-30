using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CharacterHitDamage : MonoBehaviour, IDamagable
{
    [SerializeField] private CharacterHealth _attachedHealth;
    [SerializeField, Min(0f)] private float _damageMultiplier = 1f;

    public void TakeDamage(int damage)
    {
        _attachedHealth.TakeDamage(Mathf.RoundToInt(damage * _damageMultiplier));
    }
}