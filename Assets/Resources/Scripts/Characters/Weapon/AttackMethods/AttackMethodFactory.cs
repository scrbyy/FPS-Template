using UnityEngine;

public class AttackMethodFactory
{
    private float _distance;
    private Transform _origin;

    public AttackMethodFactory(Transform origin, float distance)
    {
       _distance = distance;
        _origin = origin;
    }

    public IAttackMethod CreateAttackMethod(AttackMethod attackMethod)
    {
        if(attackMethod == AttackMethod.Raycast) return new RaycastAttackMethod(_origin, _distance);
        if(attackMethod == AttackMethod.Spherecast) return new SpherecastAttackMethod(_origin, _distance);
        
        else return null;
    }
}