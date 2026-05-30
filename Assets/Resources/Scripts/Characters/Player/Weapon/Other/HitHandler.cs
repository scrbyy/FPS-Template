using UnityEngine;

public class HitHandler
{
    public void HadleShot(GameObject hitObject, float damage)
    {
        if (hitObject.TryGetComponent(out IDamagable damagable) )
        {
            damagable.TakeDamage(Mathf.RoundToInt(damage));
        }
        else
        {
            // Spawn Hole
        }
    }
}