using UnityEngine;
using Zenject;

public class TiltEffect : MonoBehaviour, IMotionEffect
{
    [Header("Tilt Settings")]
    [SerializeField] private float sideTiltIntensity = 2f;
    [SerializeField] private float tiltSmoothing = 10f;

    [SerializeField] private float minSpeedFactor = 0.5f;
    [SerializeField] private float maxSpeedFactor = 1.2f;

    [Header("Dynamic Scaling")]
    [SerializeField] private float speedThreshold = 0.2f;
    [SerializeField] private float referenceSpeed = 7f;

    [SerializeField] private float baseTiltDivider = 5f;

    [Header("References")]
    [SerializeField] private PlayerEngine playerEngine;
    [Inject] private IInputProvider _inputProvider;

    private float _targetZRotation;

    public Vector3 GetLocalOffset() => Vector3.zero;

    private void LateUpdate()
    {
        if (playerEngine == null) return;

        Vector2 inputMove = _inputProvider.GetMoveVector();
        Vector3 worldVelocity = playerEngine.GetVelocity();
        float horizontalSpeed = new Vector3(worldVelocity.x, 0, worldVelocity.z).magnitude;

        bool isMoving = inputMove != Vector2.zero && horizontalSpeed > speedThreshold;
        bool canApplyEffect = isMoving && playerEngine.isGrounded() && !playerEngine.IsImpulseActive();

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

        float speedFactor = Mathf.Clamp(speed / referenceSpeed, minSpeedFactor, maxSpeedFactor);

        float targetTilt = -localVelocity.x * (sideTiltIntensity / baseTiltDivider) * speedFactor;

        _targetZRotation = Mathf.Lerp(_targetZRotation, targetTilt, Time.deltaTime * tiltSmoothing);

        ApplyZRotation(_targetZRotation);
    }

    private void ResetTilt()
    {
        _targetZRotation = Mathf.Lerp(_targetZRotation, 0, Time.deltaTime * tiltSmoothing);
        ApplyZRotation(_targetZRotation);
    }

    private void ApplyZRotation(float zAngle)
    {
        Vector3 currentEuler = transform.localEulerAngles;
        transform.localRotation = Quaternion.Euler(currentEuler.x, currentEuler.y, zAngle);
    }
}