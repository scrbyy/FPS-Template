using UnityEngine;

public class WeaponData : ScriptableObject, ISpeedModifier
{
    public float Damage => _damage;
    public float SpeedMultiplier => _speedMultipler;
    public float OpenDelay => _openDelay;
    public AttackMethod AttackMethod => _attackMethod;
    public float AfterAttackDelay => _afterAttackDelay;

    [SerializeField] private float _damage;
    [SerializeField] private float _speedMultipler;
    [SerializeField] private float _openDelay;
    [SerializeField] private float _afterAttackDelay;
    [SerializeField] private AttackMethod _attackMethod;
}