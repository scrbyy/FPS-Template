using UnityEngine;

public class InteractionBody : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private float maxDistance;

    [SerializeField] private LayerMask mask;
    [SerializeField] private Transform rayOrigin;

    [Header("References")]
    [SerializeField] private InputProvider selectedInputProvider;

    private RaycastHit hit;

    private void CheckInteraction()
    {
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, maxDistance, mask))
        {
            IInteractionObject _interactionObject = hit.collider.gameObject.GetComponent<IInteractionObject>();
            _interactionObject.Use();
        }
    }

    private void OnEnable()
    {
        selectedInputProvider.OnInteractPerformed += CheckInteraction;
    }

    private void OnDisable()
    {
        selectedInputProvider.OnInteractPerformed -= CheckInteraction;
    }
}