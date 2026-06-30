using UnityEngine;

public class HitHandler
{
    public void HadleShot(HitData hitData, float damage, GameObject decal)
    {
        if (hitData.hitObject.TryGetComponent(out IDamagable damagable) )
        {
            damagable.TakeDamage(Mathf.RoundToInt(damage));
        }
        Quaternion decalRotation = Quaternion.LookRotation(-hitData.normal);

        Vector3 spawnPosition = hitData.hitPoint + (hitData.normal * 0.2f);

        GameObject instantiatedDecal = Object.Instantiate(decal, spawnPosition, decalRotation);
        instantiatedDecal.transform.SetParent(hitData.hitObject.transform);
    }
}