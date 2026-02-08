using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerEngine : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float accelerationRate;
    [SerializeField] private float decelerationRate;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float airCap;

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

    public bool IsMoving(float threshold = 0.1f)
    {
        Vector3 horizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
        return horizontalVelocity.magnitude > threshold;
    }

    public void AddForce(Vector3 force)
    {
        _velocity += force;
        _isImpulseActive = true;
    }

    public Vector3 GetVelocity() => _velocity;

    public bool IsImpulseActive() => _isImpulseActive;

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


}