using UnityEngine;
using Zenject;

public abstract class AttackMethodData
{
    public abstract AttackType Type { get; }

    public abstract IAttackMethod CreateMethod(Transform origin, DiContainer container);
}