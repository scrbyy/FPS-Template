using UnityEngine;
using Zenject;
public class InteractionBody : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private float maxDistance;

    [SerializeField] private LayerMask mask;
    [SerializeField] private Transform rayOrigin;

    [Header("References")]
    [Inject] private IInteractionInputProvider selectedInputProvider;

    private RaycastHit hit;

    private void CheckInteraction()
    {
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, maxDistance, mask))
        {
            IInteractionObject _interactionObject = hit.collider.gameObject.GetComponent<IInteractionObject>();
            _interactionObject.Interact();
        }
    }

    private void OnEnable()
    {
        selectedInputProvider.OnInteractStarted += CheckInteraction;
    }

    private void OnDisable()
    {
        selectedInputProvider.OnInteractStarted -= CheckInteraction;
    }
}