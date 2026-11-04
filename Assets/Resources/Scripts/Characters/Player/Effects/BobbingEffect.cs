using Zenject;
using UnityEngine;

public class BobbingEffect : PositionEffect
{
    [Header("Intensity Limitations")]
    [SerializeField] private float _minIntensity;
    [SerializeField] private float _maxIntensity;

    [Header("Intensity Settings")]
    [SerializeField] private float _verticalIntensity;
    [SerializeField] private float _horizontalIntensity;

    [Header("Speed Modifiers")]
    [SerializeField] private float _intensityMultiplier;
    [SerializeField] private float _stepSpeedMultiplier;

    [Header("Bobbing Curve")]
    [SerializeField] private AnimationCurve _stepCurve;

    [Header("Dynamic Scaling")]
    [SerializeField] private float _baseStepRate;

    [Space]
    [SerializeField] private float _minSpeedThreshold;

    [Space]
    [SerializeField] private float _resetTime;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;

    [Inject] private IGroundChecker _groundChecker;
    [Inject] private IMovementInputProvider _inputProvider;

    private float _cycleTimer;
    private Vector3 _currentCalculatedOffset;
    private Vector3 _targetBobOffset;
    private Vector3 _resetVelocity;

    private const float HalfCycleMultiplier = 0.5f;
    private const float CurveNormalizationOffset = 0.5f;
    private const float CurveNormalizationScale = 2f;

    public override Vector3 GetLocalOffset() => _currentCalculatedOffset;

    private void LateUpdate()
    {
        if (!_characterEngine.IsImpulseActive)
        {
            Vector2 inputMove = _inputProvider.MoveInput;
            Vector3 worldVelocity = _characterEngine.Velocity;
            float horizontalSpeed = new Vector3(worldVelocity.x, 0, worldVelocity.z).magnitude;

            bool isMoving = inputMove != Vector2.zero && horizontalSpeed > _minSpeedThreshold;
            bool canApplyEffect = isMoving && _groundChecker.IsGrounded && !_characterEngine.IsImpulseActive;

            if (canApplyEffect)
            {
                float currentStepRate = _baseStepRate + (Mathf.Sqrt(horizontalSpeed) * _stepSpeedMultiplier);
                _cycleTimer += Time.deltaTime * currentStepRate;

                _targetBobOffset = CalculateBobbingTarget(horizontalSpeed);
            }
            else
            {
                _targetBobOffset = Vector3.zero;
                _cycleTimer = 0f;
            }

            _currentCalculatedOffset = Vector3.SmoothDamp(
                _currentCalculatedOffset,
                _targetBobOffset,
                ref _resetVelocity,
                _resetTime
            );
        }
    }

    private Vector3 CalculateBobbingTarget(float speed)
    {
        float waveX = Mathf.Sin(_cycleTimer * HalfCycleMultiplier);
        float waveY = Mathf.Sin(_characterEngine.Velocity.magnitude > 0 ? _cycleTimer : 0); 

        float normalizedCurveX = _stepCurve.Evaluate((waveX + 1f) * CurveNormalizationOffset) * CurveNormalizationScale - 1f;
        float normalizedCurveY = _stepCurve.Evaluate((waveY + 1f) * CurveNormalizationOffset) * CurveNormalizationScale - 1f;

        float speedFactor = Mathf.Clamp(speed * _intensityMultiplier, _minIntensity, _maxIntensity);

        return new Vector3(
            normalizedCurveX * _horizontalIntensity * speedFactor,
            normalizedCurveY * _verticalIntensity * speedFactor,
            0
        );
    }
}