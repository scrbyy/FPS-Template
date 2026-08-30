using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CharacterHitDamage : MonoBehaviour, IHittable
{
    [SerializeField] private CharacterHealth _attachedHealth;
    [SerializeField, Min(0f)] private float _damageMultiplier = 1f;

    public void OnHit(int damage)
    {
        _attachedHealth.TakeDamage(Mathf.RoundToInt(damage * _damageMultiplier));
    }
}