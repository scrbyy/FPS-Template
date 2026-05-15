using UnityEngine;
using Zenject;

public class TiltEffect : MonoBehaviour, IMotionEffect
{
    [Header("Tilt Settings")]
    [SerializeField] private float sideTiltIntensity;
    [SerializeField] private float tiltSmoothing;

    [SerializeField] private float minSpeedFactor;
    [SerializeField] private float maxSpeedFactor;

    [Header("Dynamic Scaling")]
    [SerializeField] private float speedThreshold;
    [SerializeField] private float referenceSpeed;

    [SerializeField] private float baseTiltDivider;

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