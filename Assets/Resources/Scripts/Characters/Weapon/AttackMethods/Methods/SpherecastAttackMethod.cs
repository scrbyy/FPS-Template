using UnityEngine;

public class SpherecastAttackMethod : IAttackMethod
{
    private float _radius;
    private Transform _origin;
    private float _distance;
    private LayerMask _ignoreLayer;

    private HitData _hitData;
    private RaycastHit _hit;

    public SpherecastAttackMethod(Transform origin, float radius, float distance, LayerMask ignoreLayer)
    {
        _origin = origin;
        _radius = radius;
        _distance = distance;
        _ignoreLayer = ignoreLayer;
    }

    public HitData Execute()
    {
        _hitData.Origin = _origin.position;

        if(Physics.SphereCast(_origin.position, _radius, _origin.forward, out _hit, _distance, ~_ignoreLayer))
        {
            _hitData.IsHit = true;
            _hitData.SetData(_hit);
        }
        else _hitData.IsHit = false;
        return _hitData; 
    }
}