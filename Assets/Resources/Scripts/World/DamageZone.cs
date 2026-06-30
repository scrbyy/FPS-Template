using UnityEngine;

[RequireComponent(typeof(Collider))]

public class DamageZone : MonoBehaviour
{
    [SerializeField] private float _damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<CharacterHealth>()?.Decrease(_damageAmount);
        }
    }
}
