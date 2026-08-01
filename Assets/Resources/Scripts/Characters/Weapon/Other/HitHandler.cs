using UnityEngine;

public class HitHandler
{
    public void HandleShot(HitData hitData, float damage, GameObject decal)
    {
        if (hitData.hitObject.TryGetComponent(out IDamagable damagable) )
        {
            damagable.TakeDamage(Mathf.RoundToInt(damage));
        }
    }
}