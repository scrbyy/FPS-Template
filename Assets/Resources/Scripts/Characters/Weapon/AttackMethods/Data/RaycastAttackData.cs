using System;
using UnityEngine;
using Zenject;

[Serializable]
public class RaycastAttackData : AttackData
{
    public override AttackType Type => AttackType.Raycast;

    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _ignoreMask;

    public override IAttackMethod CreateMethod(Transform origin, DiContainer container)
    {
        return new RaycastAttackMethod(origin, _distance, _ignoreMask);
    }
}