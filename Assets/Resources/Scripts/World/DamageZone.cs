using UnityEngine;

[RequireComponent(typeof(Collider))]

public class DamageZone : MonoBehaviour
{
    [SerializeField] private float damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<CharacterHealth>()?.Decrease(damageAmount);
        }
    }
}
