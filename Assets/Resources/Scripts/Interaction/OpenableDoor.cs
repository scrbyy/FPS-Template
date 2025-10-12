using System.Collections;
using UnityEngine;

public class OpenableDoor : MonoBehaviour, IInteractionObject
{
    [SerializeField] private float openAngle; 
    [SerializeField] private float openTime;   

    private float closedAngleY;
    private float openedAngleY;

    private bool isOpen = false;
    private Coroutine RotateCourutine;

    private void Awake()
    {
        closedAngleY = transform.eulerAngles.y;
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
        float startY = transform.eulerAngles.y;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newY = Mathf.LerpAngle(startY, targetY, elapsed / duration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newY, transform.eulerAngles.z);
            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetY, transform.eulerAngles.z);
        RotateCourutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isOpen)
            {
                isOpen = false;
                RotateCourutine = StartCoroutine(RotateDoor(closedAngleY, openTime));
            }
        }
    }
}
