using UnityEngine;

public class HitHandler
{
    public void HandleShot(HitData hitData, float damage)
    {
        Debug.Log(hitData.hitObject.TryGetComponent(out IDamagable damagable1) + " " + hitData.hitObject.name);
        if (hitData.hitObject.TryGetComponent(out IDamagable damagable) )
        {
            Debug.Log("hit");
            damagable.TakeDamage(Mathf.RoundToInt(damage));
        }
    }
}