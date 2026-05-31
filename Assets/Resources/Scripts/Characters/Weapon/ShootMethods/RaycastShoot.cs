using UnityEngine;

public class RaycastShoot : IShootingMethod
{
    private Transform _origin;
    private float _distance;
    private RaycastHit _hit;
    private HitData _hitData;

    public RaycastShoot(Transform origin, float distance)
    {
        _origin = origin;
        _distance = distance;
    }

    public HitData ExecuteShoot()
    {
        if (Physics.Raycast(_origin.position, _origin.forward, out _hit, _distance))
        {
            GameObject hitObject = _hit.transform.gameObject;
            _hitData.isHit = true;
            _hitData.originPoint = _origin.position;
            _hitData.hitPoint = _hit.point;
            _hitData.hitObject = hitObject.transform.gameObject;
            _hitData.normal= _hit.normal;
        }

        else _hitData.isHit = false;

        return _hitData;
    }
}