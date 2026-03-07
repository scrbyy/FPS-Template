using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFovEffect : MonoBehaviour
{
    [Header("FOV Limits")]
    [SerializeField] private float _minSpeedThreshold;  
    [SerializeField] private float _maxSpeedThreshold;

    [Space]
    [SerializeField] private float _maxFovAdd;  
    [SerializeField] private float _decreaseSpeed;         
    [SerializeField] private float _increaseSpeed;

    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private PlayerEngine _playerEngine;

    private float _defaultFov;
    private Vector3 _lastFramePosition;

    private void Awake()
    {
        _defaultFov = _camera.fieldOfView;
        _lastFramePosition = new Vector2(_bodyTransform.position.x, _bodyTransform.position.z);
    }

    private void LateUpdate()
    {
        Vector3 direction = (_lastFramePosition - _bodyTransform.position).normalized;

        Vector3 velocity = _playerEngine.GetVelocity();

        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        float dot = Vector3.Dot(transform.TransformDirection(direction), -(transform.TransformDirection(transform.forward)));

        float directionModifier = Mathf.Max(0, dot);

        float modifier = Mathf.InverseLerp(_minSpeedThreshold, _maxSpeedThreshold, horizontalSpeed);

        float targetFov = _defaultFov + (modifier * _maxFovAdd) * directionModifier;

        float currentLerp = (targetFov > _camera.fieldOfView) ? _increaseSpeed : _decreaseSpeed;

        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFov, Time.deltaTime * currentLerp);

        _lastFramePosition = _bodyTransform.position;
    }
}