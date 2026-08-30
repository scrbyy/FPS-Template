using UnityEngine;

public class HitHandler
{
    public void HandleShot(HitData hitData, float damage)
    {
        if (hitData.GameObject.TryGetComponent(out IHittable target) )
        {
            target.OnHit(Mathf.RoundToInt(damage));
        }
    }
}