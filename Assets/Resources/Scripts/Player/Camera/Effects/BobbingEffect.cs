using UnityEngine;

public class BobbingEffect : MonoBehaviour, IHeadEffect
{
    [Header("Bobbing Settings")]
    [SerializeField] private AnimationCurve bobCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float verticalAmplitude = 0.05f;
    [SerializeField] private float horizontalAmplitude = 0.03f;
    [SerializeField] private float baseFrequency = 5f;
    [SerializeField] private float frequencyStiffness = 2f;

    [Header("Tilt Settings")]
    [SerializeField] private float tiltAmplitude = 2f;
    [SerializeField] private float tiltSmoothSpeed = 10f;

    [Header("References")]
    [SerializeField] private PlayerEngine playerEngine;

    private float _timer;
    private Vector3 _bobOffset;
    private float _currentTiltZ;

    public Vector3 GetLocalOffset() => _bobOffset;

    private void LateUpdate()
    {
        if (playerEngine == null) return;

        Vector3 velocity = playerEngine.GetVelocity();
        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;
        bool isDashing = playerEngine.IsImpulseActive();

        if (horizontalSpeed > 0.2f && !isDashing)
        {
            float dynamicFrequency = baseFrequency + (Mathf.Sqrt(horizontalSpeed) * frequencyStiffness);
            _timer += Time.deltaTime * dynamicFrequency;

            UpdateBobbingValues(horizontalSpeed);
        }
        else
        {
            _bobOffset = Vector3.MoveTowards(_bobOffset, Vector3.zero, Time.deltaTime * 0.5f);
        }

        HandleTilt(velocity);
    }

    private void UpdateBobbingValues(float speed)
    {
        float waveX = Mathf.Sin(_timer * 0.5f);
        float waveY = Mathf.Sin(_timer);

        float curveX = bobCurve.Evaluate((waveX + 1f) * 0.5f) * 2f - 1f;
        float curveY = bobCurve.Evaluate((waveY + 1f) * 0.5f) * 2f - 1f;

        float ampFactor = Mathf.Clamp(speed / 7f, 0.5f, 1.2f);

        _bobOffset = new Vector3(
            curveX * horizontalAmplitude * ampFactor,
            curveY * verticalAmplitude * ampFactor,
            0
        );
    }

    private void HandleTilt(Vector3 velocity)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float targetTilt = -localVelocity.x * (tiltAmplitude / 5f);
        _currentTiltZ = Mathf.Lerp(_currentTiltZ, targetTilt, Time.deltaTime * tiltSmoothSpeed);

        Vector3 rot = transform.localEulerAngles;
        transform.localRotation = Quaternion.Euler(rot.x, rot.y, _currentTiltZ);
    }
}