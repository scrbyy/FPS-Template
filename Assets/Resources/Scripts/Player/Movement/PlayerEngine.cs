using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerEngine : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float accelerationRate = 50f;
    [SerializeField] private float decelerationRate = 40f;
    [SerializeField] private float airAcceleration = 30f;
    [SerializeField] private float airCap = 2f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isImpulseActive;

    private void Awake() => _controller = GetComponent<CharacterController>();

    public void Move(Vector3 wishDir, float maxSpeed, float verticalVelocity)
    {
        if (_controller.isGrounded)
            ApplyGroundMovement(wishDir, maxSpeed);
        else
            ApplyAirMovement(wishDir);

        Vector3 finalMotion = _velocity;
        finalMotion.y = verticalVelocity;

        _controller.Move(finalMotion * Time.deltaTime);
    }

    private void ApplyGroundMovement(Vector3 wishDir, float maxSpeed)
    {
        float targetSpeed = wishDir.magnitude * maxSpeed;

        if (targetSpeed > 0.01f) _isImpulseActive = false;

        if (_isImpulseActive)
        {
            _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, decelerationRate * Time.deltaTime);
            if (_velocity.magnitude <= maxSpeed) _isImpulseActive = false;
            return;
        }

        float accel = (targetSpeed > 0 ? accelerationRate : decelerationRate);
        _velocity = Vector3.MoveTowards(_velocity, wishDir * targetSpeed, accel * Time.deltaTime);
    }

    private void ApplyAirMovement(Vector3 wishDir)
    {
        if (wishDir.magnitude <= 0) return;

        float currentSpeedInWishDir = Vector3.Dot(_velocity, wishDir);
        float addSpeed = Mathf.Max(0, airCap - currentSpeedInWishDir);

        if (addSpeed > 0)
        {
            float accelSpeed = Mathf.Min(airAcceleration * Time.deltaTime, addSpeed);
            _velocity += wishDir * accelSpeed;
        }
    }

    public void AddForce(Vector3 force)
    {
        _velocity += force;
        _isImpulseActive = true;
    }

    public Vector3 GetVelocity() => _velocity;
}