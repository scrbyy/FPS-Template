using UnityEngine;

public class RaycastAttackMethod : IAttackMethod
{
    private Transform _origin;
    private float _distance;
    private LayerMask _ignoreLayer;

    private RaycastHit _hit;
    private HitData _hitData;

    public RaycastAttackMethod(Transform origin, float distance, LayerMask ignoreLayer)
    {
        _origin = origin;
        _distance = distance;
        _ignoreLayer = ignoreLayer;
    }

    public HitData Execute()
    {
        _hitData.Origin = _origin.position;
        if (Physics.Raycast(_origin.position, _origin.forward, out _hit, _distance, ~_ignoreLayer))
        {
            _hitData.IsHit = true;
            _hitData.SetData(_hit);
        }
        else _hitData.IsHit = false;

        return _hitData;
    }
}