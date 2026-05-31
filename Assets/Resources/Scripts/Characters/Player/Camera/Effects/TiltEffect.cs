using Zenject;
using UnityEngine;

public class TiltEffect : MonoBehaviour, IMotionEffect
{
    [Header("Tilt Settings")]
    [SerializeField] private float _sideTiltIntensity;
    [SerializeField] private float _tiltSmoothing;

    [SerializeField] private float _minSpeedFactor;
    [SerializeField] private float _maxSpeedFactor;

    [Header("Dynamic Scaling")]
    [SerializeField] private float _speedThreshold;
    [SerializeField] private float _referenceSpeed;

    [SerializeField] private float _baseTiltDivider;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;

    [Inject] private IMovementInputProvider _inputProvider;
    [Inject] private IGroundChecker _groundCheck;

    private float _targetZRotation;

    public Vector3 GetLocalOffset() => Vector3.zero;

    private void LateUpdate()
    {
        Vector2 inputMove = _inputProvider.MoveInput;
        Vector3 worldVelocity = _characterEngine.Velocity;
        float horizontalSpeed = new Vector3(worldVelocity.x, 0, worldVelocity.z).magnitude;

        bool isMoving = inputMove != Vector2.zero && horizontalSpeed > _speedThreshold;
        bool canApplyEffect = isMoving && _groundCheck.IsGrounded && !_characterEngine.IsImpulseActive();
        
        if (canApplyEffect)
        {
            UpdateSideTilt(worldVelocity, horizontalSpeed);
        }
        else
        {
            ResetTilt();
        }
    }

    private void UpdateSideTilt(Vector3 velocity, float speed)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float speedFactor = Mathf.Clamp(speed / _referenceSpeed, _minSpeedFactor, _maxSpeedFactor);

        float targetTilt = -localVelocity.x * (_sideTiltIntensity / _baseTiltDivider) * speedFactor;

        _targetZRotation = Mathf.Lerp(_targetZRotation, targetTilt, Time.deltaTime * _tiltSmoothing);

        ApplyZRotation(_targetZRotation);
    }

    private void ResetTilt()
    {
        _targetZRotation = Mathf.Lerp(_targetZRotation, 0, Time.deltaTime * _tiltSmoothing);
        ApplyZRotation(_targetZRotation);
    }

    private void ApplyZRotation(float zAngle)
    {
        Vector3 currentEuler = transform.localEulerAngles;
        transform.localRotation = Quaternion.Euler(currentEuler.x, currentEuler.y, zAngle);
    }
}