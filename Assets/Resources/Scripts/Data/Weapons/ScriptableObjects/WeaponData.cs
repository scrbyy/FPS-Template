using UnityEngine;

public class WeaponData : ScriptableObject, ISpeedModifier, IAttackData
{
    public float Damage => _damage;
    public float SpeedMultiplier => _speedMultipler;
    public float OpenDelay => _openDelay;
    public FireMode FireMode => _fireMode;
    public float AfterAttackDelay => _afterAttackDelay;

    public float MaxDistance => throw new System.NotImplementedException();

    public AttackType AttackMethod => throw new System.NotImplementedException();

    [SerializeField] private float _damage;
    [SerializeField] private float _speedMultipler;
    [SerializeField] private float _openDelay;
    [SerializeField] private float _afterAttackDelay;
    [SerializeField] private FireMode _fireMode;

    [SerializeField] private AttackType _attackMethod;
    [SerializeField] private RaycastAttackData _raycastParams;
    [SerializeField] private SpherecastAttackData _spherecastParams;

    public AttackData GetAttackConfig()
    {
        return _attackMethod switch
        {
            AttackType.Raycast => _raycastParams,
            AttackType.Spherecast => _spherecastParams,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }
}