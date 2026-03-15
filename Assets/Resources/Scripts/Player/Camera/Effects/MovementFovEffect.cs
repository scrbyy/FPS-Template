using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MovementFovEffect : MonoBehaviour
{
    [Header("FOV Limits")]
    [SerializeField] private float _minSpeedThreshold = 5f;
    [SerializeField] private float _maxSpeedThreshold = 20f;

    [Space]
    [SerializeField] private float _maxFovAdd = 15f;
    [SerializeField] private float _decreaseSpeed = 2f;
    [SerializeField] private float _increaseSpeed = 5f;

    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private PlayerEngine _playerEngine;

    private float _defaultFov;
    private Vector3 _lastFramePosition;

    private void Awake()
    {
        if (_camera == null) _camera = GetComponent<Camera>();
        _defaultFov = _camera.fieldOfView;
        _lastFramePosition = _bodyTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 movementDelta = _bodyTransform.position - _lastFramePosition;
        Vector3 direction = movementDelta.normalized;

        Vector3 velocity = _playerEngine.GetVelocity();
        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        float dot = Vector3.Dot(transform.forward, direction);
        float directionModifier = Mathf.Max(0, dot);

        float modifier = Mathf.InverseLerp(_minSpeedThreshold, _maxSpeedThreshold, horizontalSpeed);
        float targetFov = _defaultFov + (modifier * _maxFovAdd) * directionModifier;

        float currentLerp = (targetFov > _camera.fieldOfView) ? _increaseSpeed : _decreaseSpeed;
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFov, Time.deltaTime * currentLerp);

        _lastFramePosition = _bodyTransform.position;
    }
}