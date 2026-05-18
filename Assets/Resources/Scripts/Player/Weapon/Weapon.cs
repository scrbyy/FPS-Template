using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public event Action OnAttack;

    protected float _damage;

    public abstract void Attack();
}