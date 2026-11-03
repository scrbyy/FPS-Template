using UnityEngine;

public class WeaponData : ScriptableObject, ISpeedModifier, IAttackData
{
    public float Damage => _damage;
    public float SpeedMultiplier => _speedMultiplier;
    public float OpenDelay => _openDelay;
    public FireMode FireMode => _fireMode;
    public float AfterAttackDelay => _afterAttackDelay;
    public AttackType AttackMethod => _attackMethod;


    [Header("Attack Settings")]
    [SerializeField] private float _damage;
    [SerializeField] private float _afterAttackDelay;

    [Space]
    [SerializeField] private FireMode _fireMode;

    [Space]
    [SerializeField] private AttackType _attackMethod;

    [Header("Attack Method Settings")]
    [SerializeField] private RaycastMethodData _raycastParams;
    [SerializeField] private SpherecastMethodData _spherecastParams;

    [Header("Movement")]
    [SerializeField] private float _speedMultiplier;

    [Header("Timings")]
    [SerializeField] private float _openDelay;

    public AttackMethodData GetAttackConfig()
    {
        return _attackMethod switch
        {
            AttackType.Raycast => _raycastParams,
            AttackType.Spherecast => _spherecastParams,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }
}