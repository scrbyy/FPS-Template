using System.Collections;
using UnityEngine;

public class OpenableDoor : MonoBehaviour, IInteractionObject
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private float openAngle; 
    [SerializeField] private float openTime;   

    private float closedAngleY;
    private float openedAngleY;

    private bool isOpen = false;
    private Coroutine RotateCourutine;

    private void Awake()
    {
        closedAngleY = doorTransform.localEulerAngles.y;
        openedAngleY = closedAngleY + openAngle;
    }

    public void Use()
    {
        if (RotateCourutine != null) return;

        isOpen = !isOpen;
        RotateCourutine = StartCoroutine(RotateDoor(isOpen ? openedAngleY : closedAngleY, openTime));
    }

    private IEnumerator RotateDoor(float targetY, float duration)
    {
        float startY = doorTransform.localEulerAngles.y;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newY = Mathf.LerpAngle(startY, targetY, elapsed / duration);
            doorTransform.localEulerAngles = new Vector3(doorTransform.localEulerAngles.x, newY, doorTransform.localEulerAngles.z);
            yield return null;
        }

        doorTransform.localEulerAngles = new Vector3(doorTransform.localEulerAngles.x, targetY, doorTransform.localEulerAngles.z);
        RotateCourutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (RotateCourutine != null)
            {
                isOpen = true;
                StopCoroutine(RotateCourutine);
                RotateCourutine = null;
            }
        }
    }
}
