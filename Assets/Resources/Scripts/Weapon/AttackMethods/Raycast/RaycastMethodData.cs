using System;
using Zenject;
using UnityEngine;

[Serializable]
public class RaycastMethodData : AttackMethodData
{
    public override AttackType Type => AttackType.Raycast;

    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _ignoreMask;

    public override IAttackMethod CreateMethod(Transform origin, DiContainer container)
    {
        return new RaycastMethod(origin, _distance, _ignoreMask);
    }
}