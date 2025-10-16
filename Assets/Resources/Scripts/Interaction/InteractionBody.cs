using UnityEngine;

public class InteractionBody : MonoBehaviour
{
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask mask;
    [SerializeField] private InputProvider selectedInputProvider;

    private RaycastHit hit;

    private void Update()
    {
        if(Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, maxDistance, mask))
        {
            if (selectedInputProvider.isInteractButtonDown())
            {
                IInteractionObject _interactionObject = hit.collider.gameObject.GetComponent<IInteractionObject>();
                _interactionObject.Use();
            }
        }
    }
}
