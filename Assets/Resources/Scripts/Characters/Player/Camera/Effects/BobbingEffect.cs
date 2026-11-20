using Zenject;
using UnityEngine;

using scrbyy.FpsTemplate.InputSystem;
using scrbyy.FpsTemplate.Characters.Player.Movement;
using scrbyy.FpsTemplate.Characters.Player.Movement.Collisions.GroundChecker;

namespace scrbyy.FpsTemplate.Characters.Player.CameraSystem.Effects
{
    [RequireComponent(typeof(PositionEffectHandler))]
    public class BobbingEffect : PositionEffect
    {
        [Header("Amplitude & Shape")]
        [SerializeField] private float _verticalBob = 0.035f;
        [SerializeField] private float _horizontalBob = 0.022f;
        [SerializeField] private float _bobScaleBySpeed = 0.7f;
        [SerializeField] private float _minBobScale = 0.25f;
        [SerializeField] private float _maxBobScale = 1.6f;
        [SerializeField] private AnimationCurve _stepShapeCurve;

        [Header("Step Rhythm")]
        [SerializeField] private float _baseStepFrequency = 6.5f;
        [SerializeField] private float _speedStepFrequency = 4.5f;

        [Header("Thresholds & Smoothing")]
        [SerializeField] private float _minMoveSpeed = 0.15f;
        [SerializeField] private float _fadeOutTime = 0.18f;

        [Header("Dependencies")]
        [SerializeField] private CharacterEngine _characterEngine;

        [Inject] private IGroundChecker _groundChecker;
        [Inject] private IMovementInputProvider _inputProvider;

        private float _stepCycleProgress;
        private Vector3 _currentOffset;
        private Vector3 _targetOffset;
        private Vector3 _fadeVelocity;

        private const float HalfCycleMultiplier = 0.5f;
        private const float CurveNormalizationOffset = 0.5f;
        private const float CurveNormalizationScale = 2f;

        public override Vector3 GetLocalOffset() => _currentOffset;

        private void LateUpdate()
        {
            if (!_characterEngine.IsImpulseActive)
            {
                Vector2 moveInput = _inputProvider.MoveInput;
                Vector3 velocity = _characterEngine.Velocity;
                float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

                bool isMoving = moveInput != Vector2.zero && horizontalSpeed > _minMoveSpeed;
                bool canBob = isMoving && _groundChecker.IsGrounded;

                if (canBob)
                {
                    float stepFrequency = _baseStepFrequency + (Mathf.Sqrt(horizontalSpeed) * _speedStepFrequency);
                    _stepCycleProgress += Time.deltaTime * stepFrequency;

                    _targetOffset = CalculateTargetOffset(horizontalSpeed);
                }
                else
                {
                    _targetOffset = Vector3.zero;
                    _stepCycleProgress = 0f;
                }

                _currentOffset = Vector3.SmoothDamp(
                    _currentOffset,
                    _targetOffset,
                    ref _fadeVelocity,
                    _fadeOutTime
                );
            }
        }

        private Vector3 CalculateTargetOffset(float speed)
        {
            float waveX = Mathf.Sin(_stepCycleProgress * HalfCycleMultiplier);
            float waveY = Mathf.Sin(_characterEngine.Velocity.magnitude > 0 ? _stepCycleProgress : 0);

            float normalizedCurveX = _stepShapeCurve.Evaluate((waveX + 1f) * CurveNormalizationOffset) * CurveNormalizationScale - 1f;
            float normalizedCurveY = _stepShapeCurve.Evaluate((waveY + 1f) * CurveNormalizationOffset) * CurveNormalizationScale - 1f;

            float bobScale = Mathf.Clamp(speed * _bobScaleBySpeed, _minBobScale, _maxBobScale);

            return new Vector3(
                normalizedCurveX * _horizontalBob * bobScale,
                normalizedCurveY * _verticalBob * bobScale,
                0
            );
        }
    }
}