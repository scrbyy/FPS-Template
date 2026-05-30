using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterEngine : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _accelerationRate;
    [SerializeField] private float _decelerationRate;

    [Header("CS-Style Air Movement")]
    [SerializeField] private float _airAcceleration;
    [SerializeField] private float _airCap;

    [Header("Gravity")]
    [SerializeField] private float _gravityScale;
    [SerializeField] private float _downforce;

    private bool _isImpulseActive;

    private Vector3 _velocity;
    private CharacterController _characterController;

    public Vector3 GetVelocity() => _velocity;

    public bool IsImpulseActive() => _isImpulseActive;

    public bool IsMoving(float threshold = 0.1f)
    {
        Vector3 horizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
        return horizontalVelocity.magnitude > threshold;
    }

    public void Move(Vector3 inputVector, float maxSpeed)
    {
        if (_characterController.isGrounded) ApplyGroundMovement(inputVector, maxSpeed);
        else ApplyAirMovement(inputVector);

        Vector3 finalMotion = _velocity;
        finalMotion.y = _velocity.y;

        _characterController.Move(finalMotion * Time.deltaTime);
    }

    public void AddForce(Vector3 force, ForceType type)
    {
        if (type == ForceType.Jump)
        {
            _velocity.y = Mathf.Sqrt(force.y * _downforce * Physics.gravity.y);
        }
        else if (type == ForceType.Impulse)
        {
            _velocity += force;
            _isImpulseActive = true;
        }
    }

    private void ApplyGroundMovement(Vector3 wishDir, float maxSpeed)
    {
        float targetSpeed = wishDir.magnitude * maxSpeed;

        if (targetSpeed > 0.01f) _isImpulseActive = false;

        if (_isImpulseActive)
        {
            _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, _decelerationRate * Time.deltaTime);
            if (_velocity.magnitude <= maxSpeed) _isImpulseActive = false;
            return;
        }

        float accel = (targetSpeed > 0 ? _accelerationRate : _decelerationRate);
        _velocity = Vector3.MoveTowards(_velocity, wishDir * targetSpeed, accel * Time.deltaTime);
    }

    private void ApplyAirMovement(Vector3 wishDir)
    {
        if (wishDir.magnitude <= 0) return;

        float currentSpeedInWishDir = Vector3.Dot(_velocity, wishDir);
        float addSpeed = Mathf.Max(0, _airCap - currentSpeedInWishDir);

        if (addSpeed > 0)
        {
            float accelSpeed = Mathf.Min(_airAcceleration * Time.deltaTime, addSpeed);
            _velocity += wishDir * accelSpeed;
        }
    }

    private void Update()
    {
        ApplyGravity();
        HandleHeadHit();
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0f)
            _velocity.y = _downforce;
        else
            _velocity.y += Physics.gravity.y * _gravityScale * Time.deltaTime;
    }

    private void HandleHeadHit()
    {
        if ((_characterController.collisionFlags & CollisionFlags.Above) != 0 && _velocity.y > 0f)
        {
            _velocity.y = 0f;
        }
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
}