using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private AnimationCurve bobCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float verticalAmplitude;
    [SerializeField] private float horizontalAmplitude;
    [SerializeField] private float baseFrequency;
    [SerializeField] private float frequencyStiffness;

    [Header("Tilt Settings (Z-Axis)")]
    [SerializeField] private float tiltAmplitude;
    [SerializeField] private float dashTiltMultiplier;
    [SerializeField] private float tiltSmoothSpeed;

    [Header("References")]
    [SerializeField] private PlayerEngine playerEngine;
    [SerializeField] private InputProvider inputProvider;

    bool isDashing;

    private float _timer;
    private float _currentTiltZ;
    private Vector3 _initialPosition;

    private void Awake()
    {
        _initialPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        Vector3 velocity = playerEngine.GetVelocity();
        float horizontalSpeed = new Vector2(velocity.x, velocity.z).magnitude;

        bool isDashing = playerEngine.IsImpulseActive();

        if (horizontalSpeed > 0.2f && !isDashing)
        {
            float dynamicFrequency = baseFrequency + (Mathf.Sqrt(horizontalSpeed) * frequencyStiffness);
            _timer += Time.deltaTime * dynamicFrequency;

            HandleBobbing(horizontalSpeed);
        }
        else if (isDashing)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition, Time.deltaTime * 10f);
        }
        else
        {
            ResetMotion();
        }

        HandleTilt(velocity);
    }

    private void HandleBobbing(float speed)
    {
        float waveX = Mathf.Sin(_timer * 0.5f);
        float waveY = Mathf.Sin(_timer);

        float curveX = bobCurve.Evaluate((waveX + 1f) * 0.5f) * 2f - 1f;
        float curveY = bobCurve.Evaluate((waveY + 1f) * 0.5f) * 2f - 1f;

        float ampFactor = Mathf.Clamp(speed / 7f, 0.7f, 1.4f);

        transform.localPosition = _initialPosition + new Vector3(
            curveX * horizontalAmplitude * ampFactor,
            curveY * verticalAmplitude * ampFactor,
            0
        );
    }

    private void HandleTilt(Vector3 velocity)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float lateralSpeed = localVelocity.x;

        float targetTilt = -lateralSpeed * (tiltAmplitude / 5f);
        targetTilt = Mathf.Clamp(targetTilt, -tiltAmplitude * dashTiltMultiplier, tiltAmplitude * dashTiltMultiplier);

        _currentTiltZ = Mathf.Lerp(_currentTiltZ, targetTilt, Time.deltaTime * tiltSmoothSpeed);

        Quaternion tiltRotation = Quaternion.AngleAxis(_currentTiltZ, Vector3.forward);
        transform.localRotation = transform.localRotation * tiltRotation;
    }

    private void ResetMotion()
    {
        _timer = 0;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition, Time.deltaTime * 5f);

        _currentTiltZ = Mathf.Lerp(_currentTiltZ, 0, Time.deltaTime * 5f);

        Quaternion tiltRotation = Quaternion.AngleAxis(_currentTiltZ, Vector3.forward);
        transform.localRotation = transform.localRotation * tiltRotation;
    }
}