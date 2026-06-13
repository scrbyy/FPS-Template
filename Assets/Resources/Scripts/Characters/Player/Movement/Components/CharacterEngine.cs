using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterEngine : MonoBehaviour
{
    public Vector3 Velocity => _velocity;

    public bool IsImpulseActive => _isImpulseActive;

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
    private bool _canMove;

    private Vector3 _velocity;
    private CharacterController _characterController;

    private const float MovementThreshold = 0.1f;

    public void DisableMovement()
    {
        _canMove = false;
    }

    public void EnableMovement()
    {
        _canMove = true;
    }

    public bool IsMoving()
    {
        Vector3 horizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
        return horizontalVelocity.magnitude > MovementThreshold;
    }

    public void Move(Vector3 inputDirection, float maxSpeed)
    {
        if (_canMove)
        {
            if (_characterController.isGrounded) ApplyGroundMovement(inputDirection, maxSpeed);
            else ApplyAirMovement(inputDirection);

            Vector3 finalMotion = _velocity;
            finalMotion.y = _velocity.y;

            _characterController.Move(finalMotion * Time.deltaTime);
        }
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

    private void ApplyGroundMovement(Vector3 inputDirection, float maxSpeed)
    {
        float targetSpeed = inputDirection.magnitude * maxSpeed;

        if (targetSpeed > 0.01f) _isImpulseActive = false;

        if (_isImpulseActive)
        {
            _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, _decelerationRate * Time.deltaTime);
            if (_velocity.magnitude <= maxSpeed) _isImpulseActive = false;
            return;
        }

        float accel = (targetSpeed > 0 ? _accelerationRate : _decelerationRate);
        _velocity = Vector3.MoveTowards(_velocity, inputDirection * targetSpeed, accel * Time.deltaTime);
    }

    private void ApplyAirMovement(Vector3 inputDirection)
    {
        if (inputDirection.magnitude <= 0) return;

        float currentSpeedInWishDir = Vector3.Dot(_velocity, inputDirection);
        float addSpeed = Mathf.Max(0, _airCap - currentSpeedInWishDir);

        if (addSpeed > 0)
        {
            float accelSpeed = Mathf.Min(_airAcceleration * Time.deltaTime, addSpeed);
            _velocity += inputDirection * accelSpeed;
        }
    }

    private void Update()
    {
        if (_canMove)
        {
            ApplyGravity();
            HandleCollisions();
        }
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0f)
            _velocity.y = _downforce;
        else
            _velocity.y += Physics.gravity.y * _gravityScale * Time.deltaTime;
    }

    private void HandleCollisions()
    {
        if ((_characterController.collisionFlags & CollisionFlags.Above) != 0 && _velocity.y > 0f)
        {
            _velocity.y = 0f;
        }

        if (_isImpulseActive && (_characterController.collisionFlags & CollisionFlags.Sides) != 0)
        {
            _velocity.x = 0f;
            _velocity.z = 0f;

            _isImpulseActive = false;
        }
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _canMove = true;
    }
}