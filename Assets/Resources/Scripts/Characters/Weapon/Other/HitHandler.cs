using UnityEngine;

public class HitHandler
{
    public void HandleShot(HitData hitData, float damage)
    {
        if (hitData.GameObject.TryGetComponent(out IDamageable target) )
        {
            target.TakeDamage(Mathf.RoundToInt(damage));
        }
    }
}