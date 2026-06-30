using Zenject;
using UnityEngine;

public class InteractionBody : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private float _maxDistance;

    [SerializeField] private LayerMask _mask;
    [SerializeField] private Transform _rayOrigin;

    [Inject] private IInteractionInputProvider _inputProvider;

    private RaycastHit _hit;

    private void CheckInteraction()
    {
        if (Physics.Raycast(_rayOrigin.position, _rayOrigin.forward, out _hit, _maxDistance, _mask))
        {
            IInteractionObject _interactionObject = _hit.collider.gameObject.GetComponent<IInteractionObject>();
            _interactionObject.Interact();
        }
    }

    private void OnEnable()
    {
        _inputProvider.OnInteractStarted += CheckInteraction;
    }

    private void OnDisable()
    {
        _inputProvider.OnInteractStarted -= CheckInteraction;
    }
}