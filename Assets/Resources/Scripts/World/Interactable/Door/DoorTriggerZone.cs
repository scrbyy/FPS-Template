using UnityEngine;

using scrbyy.FpsTemplate.Interaction;

namespace scrbyy.FpsTemplate.World.Interactable.Door
{
    [RequireComponent(typeof(Collider))]
    public class DoorTriggerZone : MonoBehaviour, IInteractable
    {
        [SerializeField] private DoorOpener _doorOpener;

        private void Start()
        {
            if(gameObject.layer != LayerMask.NameToLayer("Interactable"))
            {
                Debug.LogWarning($"DoorTriggerZone '{gameObject.name}' is not on the 'Interactable' layer. Please set it to the correct layer.");
            }
        }

        public void Interact()
        {
            _doorOpener.Toggle();
        }
    }
}
