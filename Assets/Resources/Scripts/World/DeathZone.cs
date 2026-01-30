using UnityEngine;

[RequireComponent(typeof(Collider))]

public class DeathZone : MonoBehaviour
{
    [SerializeField] private float damageAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(damageAmount);
        }
    }
}
