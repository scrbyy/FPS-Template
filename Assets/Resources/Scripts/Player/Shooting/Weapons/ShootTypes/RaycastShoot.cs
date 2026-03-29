using UnityEngine;

public class RaycastShoot : IShootingMethod
{
    private Transform _origin;
    private float _distance;
    private RaycastHit _hit;

    public RaycastShoot(Transform origin, float distance)
    {
        _origin = origin;
        _distance = distance;
    }

    public void ExecuteShoot()
    {
        if (Physics.Raycast(_origin.position, _origin.forward, out _hit, _distance))
        {
            GameObject hitObject = _hit.transform.gameObject;
            Debug.Log("Hit object: " + hitObject.name);
        }
    }
}