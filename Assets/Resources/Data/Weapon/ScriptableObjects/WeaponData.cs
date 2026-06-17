using UnityEngine;

public class WeaponData : ScriptableObject
{
    public float Damage => _damage;

    [SerializeField] private float _damage;
}