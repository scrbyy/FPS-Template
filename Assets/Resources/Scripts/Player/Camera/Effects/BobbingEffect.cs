using UnityEngine;
using Zenject;

public class BobbingEffect : MonoBehaviour, IMotionEffect
{
    [Header("Scale Limitations")]
    [SerializeField] private float minEffectScale;
    [SerializeField] private float maxEffectScale;

    [Header("Settings")]
    [SerializeField] private float verticalAmplitude;
    [SerializeField] private float horizontalAmplitude;

    [Space]
    [SerializeField] private AnimationCurve motionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Dynamic Scaling")]
    [SerializeField] private float baseStepFrequency;
    [SerializeField] private float frequencySensitivity;
    [SerializeField] private float speedThreshold;
    [SerializeField] private float speedSensitivity;
    [SerializeField] private float returnToZeroSpeed;

    [Header("References")]
    [SerializeField] private PlayerEngine _playerEngine;
    [Inject] private IInputProvider _inputProvider;

    private float _cycleTimer;
    private Vector3 _currentCalculatedOffset;

    private const float HalfCycleMultiplier = 0.5f;
    private const float CurveNormalizationOffset = 0.5f;
    private const float CurveNormalizationScale = 2f;

    public Vector3 GetLocalOffset() => _currentCalculatedOffset;

    private void LateUpdate()
    {
        if (!_playerEngine.IsImpulseActive())
        {
            Vector2 inputMove = _inputProvider.GetMoveVector();
            Vector3 worldVelocity = _playerEngine.GetVelocity();
            float horizontalSpeed = new Vector3(worldVelocity.x, 0, worldVelocity.z).magnitude;

            bool isMoving = inputMove != Vector2.zero && horizontalSpeed > speedThreshold;
            bool canApplyEffect = isMoving && _playerEngine.isGrounded() && !_playerEngine.IsImpulseActive();

            if (canApplyEffect)
            {
                float stepSpeedMultiplier = baseStepFrequency + (Mathf.Sqrt(horizontalSpeed) * frequencySensitivity);
                _cycleTimer += Time.deltaTime * stepSpeedMultiplier;

                UpdateBobbingOffset(horizontalSpeed);
            }
            else
            {
                ResetToNeutralState();
            }
        }
    }

    private void UpdateBobbingOffset(float speed)
    {
        float waveX = Mathf.Sin(_cycleTimer * HalfCycleMultiplier);
        float waveY = Mathf.Sin(_cycleTimer);

        float normalizedCurveX = motionCurve.Evaluate((waveX + 1f) * CurveNormalizationOffset) * CurveNormalizationScale - 1f;
        float normalizedCurveY = motionCurve.Evaluate((waveY + 1f) * CurveNormalizationOffset) * CurveNormalizationScale - 1f;

        float speedFactor = Mathf.Clamp(speed * speedSensitivity, minEffectScale, maxEffectScale);

        _currentCalculatedOffset = new Vector3(
            normalizedCurveX * horizontalAmplitude * speedFactor,
            normalizedCurveY * verticalAmplitude * speedFactor,
            0
        );
    }

    private void ResetToNeutralState()
    {
        _cycleTimer = Mathf.Lerp(_cycleTimer, 0, Time.deltaTime * returnToZeroSpeed);
        _currentCalculatedOffset = Vector3.Lerp(_currentCalculatedOffset, Vector3.zero, Time.deltaTime * returnToZeroSpeed);
    }
}