using UnityEngine;
using Zenject;

[RequireComponent(typeof(PositionEffectHandler))]
public class FallSpringEffect : MonoBehaviour, IPositionEffect
{
    [Header("Spring Settings")]
    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _shakeDamping;
    [SerializeField] private float _effectWeight;

    [Header("Landing Impact Settings")]
    [SerializeField] private float _maxForce;
    [SerializeField] private float _minFallSpeedThreshold;
    [SerializeField] private float _forceMultiplier;

    [Header("In-Air Compression Settings")]
    [SerializeField] private float _inAirCompressionForce;
    [SerializeField] private float _maxInAirOffset;
    [SerializeField] private float _fallingForceMultiplier;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;

    [Inject] private IGroundChecker _groundCheck;

    private Vector3 _calculatedCameraOffset;
    private Vector3 _shakeVelocity;
    private float _capturedFallSpeed;

    public Vector3 GetLocalOffset() => _calculatedCameraOffset;

    private void Update()
    {
        float currentVerticalSpeed = _characterEngine.Velocity.y;

        if (currentVerticalSpeed < 0 && !_groundCheck.IsGrounded)
        {
            _capturedFallSpeed = currentVerticalSpeed;

            float fallRatio = Mathf.Abs(currentVerticalSpeed) / _minFallSpeedThreshold;
            float compressionFactor = Mathf.Clamp01(fallRatio);

            float targetAirOffset = -Mathf.Min(Mathf.Abs(currentVerticalSpeed) * _fallingForceMultiplier, _maxInAirOffset);

            _calculatedCameraOffset.y = Mathf.Lerp(_calculatedCameraOffset.y, targetAirOffset, Time.deltaTime * _inAirCompressionForce);
        }
    }

    private void LateUpdate()
    {
        Vector3 returnForce = -_returnSpeed * _calculatedCameraOffset - _shakeDamping * _shakeVelocity;
        Vector3 acceleration = returnForce / _effectWeight;

        _shakeVelocity += acceleration * Time.deltaTime;
        _calculatedCameraOffset += _shakeVelocity * Time.deltaTime;
    }

    private void ApplyLandingForce()
    {
        float absoluteFallSpeed = Mathf.Abs(_capturedFallSpeed);
        if (absoluteFallSpeed < _minFallSpeedThreshold) return;

        float finalImpactForce = Mathf.Min(absoluteFallSpeed * _forceMultiplier, _maxForce);

        _shakeVelocity.y -= finalImpactForce;
        _capturedFallSpeed = 0f;
    }

    private void OnEnable()
    {
        if (_groundCheck != null)
            _groundCheck.OnGrounded += ApplyLandingForce;
    }

    private void OnDisable()
    {
        if (_groundCheck != null)
            _groundCheck.OnGrounded -= ApplyLandingForce;
    }
}