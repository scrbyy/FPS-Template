using UnityEngine;

public class ShootingMethodFactory
{
    private float _distance;
    private Transform _origin;

    public ShootingMethodFactory(Transform origin, float distance)
    {
       _distance = distance;
        _origin = origin;
    }

    public IShootingMethod CreateShootingMethod(ShootingMethod shootingMethod)
    {
        if(shootingMethod == ShootingMethod.Raycast) return new RaycastShoot(_origin, _distance);
        else return null;
    }
}