using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MovementFovEffect : MonoBehaviour
{
    [Header("Limits")]
    [SerializeField] private float _maxFov;

    [Header("Changing Rate")]
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private float _increaseSpeed;

    [Header("FOV Thresholds")]
    [SerializeField] private float _minSpeedThreshold;
    [SerializeField] private float _maxSpeedThreshold;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;

    private Camera _camera;
    private float _defaultFov;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        _defaultFov = _camera.fieldOfView;
    }

    private void LateUpdate()
    {
        Vector3 velocity = _characterEngine.GetVelocity();
        Vector3 direction = velocity.normalized;

 
        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        float dot = Vector3.Dot(transform.forward, direction);
        float directionModifier = Mathf.Max(0, dot);

        float modifier = Mathf.InverseLerp(_minSpeedThreshold, _maxSpeedThreshold, horizontalSpeed);
        float targetFov = _defaultFov + (modifier * _maxFov) * directionModifier;

        float currentLerp = (targetFov > _camera.fieldOfView) ? _increaseSpeed : _decreaseSpeed;
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFov, Time.deltaTime * currentLerp);
    }
}