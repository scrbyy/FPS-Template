using System;
using UnityEngine;
using Zenject;

[Serializable]
public class RaycastAttackData : AttackData
{
    public override AttackType Type => AttackType.Raycast;

    [SerializeField] private float _distance;

    public override IAttackMethod CreateMethod(Transform origin, DiContainer container)
    {
        return new RaycastAttackMethod(origin, _distance);
    }
}