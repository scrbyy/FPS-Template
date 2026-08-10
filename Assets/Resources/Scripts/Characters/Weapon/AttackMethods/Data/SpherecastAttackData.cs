using System;
using UnityEngine;
using Zenject;

[Serializable]
public class SpherecastAttackData : AttackData
{
    public override AttackType Type => AttackType.Spherecast;

    [SerializeField] private float _distance;
    [SerializeField] private float _radius;

    public override IAttackMethod CreateMethod(Transform origin, DiContainer container)
    {
        return new SpherecastAttackMethod(origin, _radius, _distance);
    }
}