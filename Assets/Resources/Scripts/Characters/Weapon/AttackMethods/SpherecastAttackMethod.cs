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
        _hitData.origin = _origin.position;

        if(Physics.SphereCast(_origin.position, _radius, _origin.forward, out _hit))
        {
            _hitData.isHit = true;
            _hitData.SetData(_hit);
        }
        else _hitData.isHit = false;

        return _hitData; 
    }
}