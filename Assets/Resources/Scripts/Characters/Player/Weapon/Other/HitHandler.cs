using UnityEngine;

public class HitHandler
{
    public void HadleShot(GameObject hitObject, float damage)
    {
        if (hitObject.TryGetComponent(out IDamagable damagable) )
        {
            damagable.TakeDamage(damage);
        }
        else
        {
            // Spawn Hole
        }
    }
}