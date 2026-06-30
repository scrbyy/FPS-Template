using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Action OnAttack;

    protected float _damage;

    [SerializeField] protected CharacterSpeed _ownerSpeedHandler;

    public abstract void Attack();

    public abstract void Initialize();

    public abstract void Deinitialize();
}