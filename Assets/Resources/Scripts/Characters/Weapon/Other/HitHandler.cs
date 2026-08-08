using UnityEngine;

public class HitHandler
{
    public void HandleShot(HitData hitData, float damage)
    {
        if (hitData.gameobject.TryGetComponent(out IDamagable damagable) )
        {
            damagable.TakeDamage(Mathf.RoundToInt(damage));
        }
    }
}