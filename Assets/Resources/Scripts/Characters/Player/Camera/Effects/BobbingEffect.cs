using UnityEngine;
using Zenject;

public class BobbingEffect : MonoBehaviour, IMotionEffect
{
    [Header("Scale Limitations")]
    [SerializeField] private float _minEffectScale;
    [SerializeField] private float _maxEffectScale;

    [Header("Settings")]
    [SerializeField] private float _verticalAmplitude;
    [SerializeField] private float _horizontalAmplitude;

    [Header("Bobbing Curve")]
    [SerializeField] private AnimationCurve _motionCurve;

    [Header("Dynamic Scaling")]
    [SerializeField] private float _stepFrequency;
    [SerializeField] private float _frequencySensitivity;
    [SerializeField] private float _speedThreshold;
    [SerializeField] private float _speedSensitivity;

    [SerializeField] private float _returnToZeroSpeed;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;

    [Inject] private IGroundChecker _groundChecker;
    [Inject] private IMovementInputProvider _inputProvider;

    private float _cycleTimer;
    private Vector3 _currentCalculatedOffset;
    private Vector3 _targetBobOffset;

    private const float HalfCycleMultiplier = 0.5f;
    private const float CurveNormalizationOffset = 0.5f;
    private const float CurveNormalizationScale = 2f;

    public Vector3 GetLocalOffset() => _currentCalculatedOffset;

    private void LateUpdate()
    {
        if (!_characterEngine.IsImpulseActive)
        {
            Vector2 inputMove = _inputProvider.MoveInput;
            Vector3 worldVelocity = _characterEngine.Velocity;
            float horizontalSpeed = new Vector3(worldVelocity.x, 0, worldVelocity.z).magnitude;

            bool isMoving = inputMove != Vector2.zero && horizontalSpeed > _speedThreshold;
            bool canApplyEffect = isMoving && _groundChecker.IsGrounded && !_characterEngine.IsImpulseActive;

            if (canApplyEffect)
            {
                float stepSpeedMultiplier = _stepFrequency + (Mathf.Sqrt(horizontalSpeed) * _frequencySensitivity);
                _cycleTimer += Time.deltaTime * stepSpeedMultiplier;

               _targetBobOffset = CalculateBobbingTarget(horizontalSpeed);
            }
            else
            {_targetBobOffset = Vector3.zero;
                _cycleTimer = 0f;
            }
            _currentCalculatedOffset = Vector3.Lerp(_currentCalculatedOffset, _targetBobOffset, Time.deltaTime * _returnToZeroSpeed);
        }
    }

    private Vector3 CalculateBobbingTarget(float speed)
    {
        float waveX = Mathf.Sin(_cycleTimer * HalfCycleMultiplier);
        float waveY = Mathf.Sin(_characterEngine.Velocity.magnitude > 0 ? _cycleTimer : 0); 

        float normalizedCurveX = _motionCurve.Evaluate((waveX + 1f) * CurveNormalizationOffset) * CurveNormalizationScale - 1f;
        float normalizedCurveY = _motionCurve.Evaluate((waveY + 1f) * CurveNormalizationOffset) * CurveNormalizationScale - 1f;

        float speedFactor = Mathf.Clamp(speed * _speedSensitivity, _minEffectScale, _maxEffectScale);

        return new Vector3(
            normalizedCurveX * _horizontalAmplitude * speedFactor,
            normalizedCurveY * _verticalAmplitude * speedFactor,
            0
        );
    }
}