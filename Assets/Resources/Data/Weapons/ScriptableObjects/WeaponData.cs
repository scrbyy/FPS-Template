using UnityEngine;

public class WeaponData : ScriptableObject, ISpeedModifier
{
    public float Damage => _damage;

    public float SpeedMultiplier => _speedMultipler;

    public float OpenTime => _openTime;

    [SerializeField] private float _damage;

    [SerializeField] private float _speedMultipler;

    [SerializeField] private float _openTime;
}