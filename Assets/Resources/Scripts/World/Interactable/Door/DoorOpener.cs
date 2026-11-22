using System.Collections;
using UnityEngine;

namespace scrbyy.FpsTemplate.World.Interactable.Door
{
    public class DoorOpener : MonoBehaviour
    {
        [SerializeField] private float _openAngle;
        [SerializeField] private float _openTime;

        [Header("References")]
        [SerializeField] private Transform _doorTransform;

        private float _closedAngleY;
        private float _openedAngleY;

        private bool _isOpen = false;
        private Coroutine _openCoroutine;

        private void Awake()
        {
            _closedAngleY = _doorTransform.localEulerAngles.y;
            _openedAngleY = _closedAngleY + _openAngle;
        }

        public void Toggle()
        {
            if (_openCoroutine != null) return;

            _isOpen = !_isOpen;
            _openCoroutine = StartCoroutine(RotateDoor(_isOpen ? _openedAngleY : _closedAngleY, _openTime));
        }

        private IEnumerator RotateDoor(float targetY, float duration)
        {
            float startY = _doorTransform.localEulerAngles.y;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float newY = Mathf.LerpAngle(startY, targetY, elapsed / duration);
                _doorTransform.localEulerAngles = new Vector3(_doorTransform.localEulerAngles.x, newY, _doorTransform.localEulerAngles.z);
                yield return null;
            }

            _doorTransform.localEulerAngles = new Vector3(_doorTransform.localEulerAngles.x, targetY, _doorTransform.localEulerAngles.z);
            _openCoroutine = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (_openCoroutine != null)
                {
                    _isOpen = true;
                    StopCoroutine(_openCoroutine);
                    _openCoroutine = null;
                }
            }
        }
    }
}