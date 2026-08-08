using UnityEngine;

public class SpherecastAttackMethod : IAttackMethod
{
    private float _radius;
    private Transform _origin;

    private HitData _hitData;
    private RaycastHit _hit;

    public SpherecastAttackMethod(Transform origin, float radius)
    {
        _origin = origin;
        _radius = radius;
    }

    public HitData Execute()
    {
        _hitData.originPoint = _origin.position;

        if(Physics.SphereCast(_origin.position, _radius, _origin.forward, out _hit))
        {
            _hitData.isHit = true;
            _hitData.hitPoint = _hit.point;
            _hitData.hitObject = _hit.transform.gameObject;
            _hitData.normal = _hit.normal;
        }
        else _hitData.isHit = false;

        return _hitData; 
    }
}