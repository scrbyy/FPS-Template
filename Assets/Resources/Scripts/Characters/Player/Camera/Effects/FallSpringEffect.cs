using UnityEngine;
using Zenject;

public class FallSpringEffect : MonoBehaviour, IMotionEffect
{
    [Header("Spring Settings")]
    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _shakeDamping;
    [SerializeField] private float _effectWeight;   

    [Header("Dynamic Scaling")]
    [SerializeField] private float _maxForce;
    [SerializeField] private float _minFallSpeedThreshold;
    [SerializeField] private float _forceMultiplier;

    [Header("References")]
    [SerializeField] private CharacterEngine _playerEngine;

    [Inject] private IGroundChecker _groundCheck;

    private Vector3 _calculatedCameraOffset;
    private Vector3 _shakeVelocity;
    private float _capturedFallSpeed;
    
    public Vector3 GetLocalOffset() => _calculatedCameraOffset;

    private void ApplyLandingForce()
    {
        float absoluteFallSpeed = Mathf.Abs(_capturedFallSpeed);
        if (absoluteFallSpeed < _minFallSpeedThreshold) return;

        float finalImpactForce = Mathf.Min(absoluteFallSpeed * _forceMultiplier, _maxForce);

        _shakeVelocity.y -= finalImpactForce;
        _capturedFallSpeed = 0f;
    }

    private void LateUpdate()
    {
        Vector3 returnForce = -_returnSpeed * _calculatedCameraOffset - _shakeDamping * _shakeVelocity;
        Vector3 acceleration = returnForce / _effectWeight;

        _shakeVelocity += acceleration * Time.deltaTime;
        _calculatedCameraOffset += _shakeVelocity * Time.deltaTime;
    }

    private void Update()
    {
        float currentVerticalSpeed = _playerEngine.GetVelocity().y;

        if (currentVerticalSpeed < 0)
        {
            _capturedFallSpeed = currentVerticalSpeed;
        }
    }

    private void OnEnable() => _groundCheck.OnGrounded += ApplyLandingForce;
    
    private void OnDisable() => _groundCheck.OnGrounded -= ApplyLandingForce;
}
