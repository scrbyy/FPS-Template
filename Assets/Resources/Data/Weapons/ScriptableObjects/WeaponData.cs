using UnityEngine;

public class WeaponData : ScriptableObject, ISpeedModifier, IAttackData
{
    public float Damage => _damage;
    public float SpeedMultiplier => _speedMultipler;
    public float OpenDelay => _openDelay;
    public AttackMethod AttackMethod => _attackMethod;
    public FireMode FireMode => _fireMode;
    public float AfterAttackDelay => _afterAttackDelay;
    public float MaxDistance => _maxDistance;

    [SerializeField] private float _damage;
    [SerializeField] private float _speedMultipler;
    [SerializeField] private float _openDelay;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _afterAttackDelay;
    [SerializeField] private AttackMethod _attackMethod;
    [SerializeField] private FireMode _fireMode;
}