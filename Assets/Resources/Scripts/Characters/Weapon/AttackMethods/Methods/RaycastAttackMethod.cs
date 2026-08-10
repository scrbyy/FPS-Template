using UnityEngine;

public class RaycastAttackMethod : IAttackMethod
{
    private Transform _origin;
    private float _distance;
    private LayerMask _ignoreLayer;

    private RaycastHit _hit;
    private HitData _hitData;

    public RaycastAttackMethod(Transform origin, float distance, LayerMask _ignoreLayer)
    {
        _origin = origin;
        _distance = distance;
    }

    public HitData Execute()
    {
        _hitData.origin = _origin.position;
        if (Physics.Raycast(_origin.position, _origin.forward, out _hit, _distance, ~_ignoreLayer))
        {
            _hitData.isHit = true;
            _hitData.SetData(_hit);
        }
        else _hitData.isHit = false;

        return _hitData;
    }
}