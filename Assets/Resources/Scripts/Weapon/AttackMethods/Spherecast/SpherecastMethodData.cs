using System;
using UnityEngine;
using Zenject;

[Serializable]
public class SpherecastMethodData : AttackMethodData
{
    public override AttackType Type => AttackType.Spherecast;

    [SerializeField] private float _distance;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _ignoreLayer;
    public override IAttackMethod CreateMethod(Transform origin, DiContainer container)
    {
        return new SpherecastMethod(origin, _radius, _distance, _ignoreLayer);
    }
}