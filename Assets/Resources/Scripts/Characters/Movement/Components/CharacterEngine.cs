using UnityEngine;
using Zenject;

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

    [Header("References")]
    [SerializeField] private CharacterSpeed _speedProvider;

    [Inject] private CharacterCollisionHandler _characterCollisionHandler;
    [Inject] private IGroundChecker _groundChecker;

    private CharacterController _characterController;

    private bool _isImpulseActive;
    private bool _canMove;
    private Vector3 _velocity;

    private const float _movingThreshold = 0.1f;

    public void Move(Vector3 inputVector)
    {
        if (_canMove)
        {
            if (_characterController.isGrounded) ApplyGroundMovement(inputVector, _speedProvider.Speed);
            else ApplyAirMovement(inputVector);

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

    private void ApplyGroundMovement(Vector3 wishDirection, float speed)
    {
        float targetSpeed = wishDirection.magnitude * speed;

        if (targetSpeed > 0.01f) _isImpulseActive = false;

        if (_isImpulseActive)
        {
            _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, _decelerationRate * Time.deltaTime);
            if (_velocity.magnitude <= speed) _isImpulseActive = false;
            return;
        }

        float accel = (targetSpeed > 0 ? _accelerationRate : _decelerationRate);
        _velocity = Vector3.MoveTowards(_velocity, wishDirection * targetSpeed, accel * Time.deltaTime);
    }

    private void ApplyAirMovement(Vector3 wishDirection)
    {
        if (wishDirection.magnitude <= 0) return;

        float currentSpeedInWishDir = Vector3.Dot(_velocity, wishDirection);
        float addSpeed = Mathf.Max(0, _airCap - currentSpeedInWishDir);

        if (addSpeed > 0)
        {
            float accelSpeed = Mathf.Min(_airAcceleration * Time.deltaTime, addSpeed);
            _velocity += wishDirection * accelSpeed;
        }
    }

    private void ApplyGravity()
    {
        if (_groundChecker.IsGrounded && _velocity.y < 0f)
            _velocity.y = _downforce;
        else
            _velocity.y += Physics.gravity.y * _gravityScale * Time.deltaTime;
    }

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
        return horizontalVelocity.magnitude > _movingThreshold;
    }

    private void Update()
    {
        if (_canMove)
        {
            ApplyGravity();
        }
        HandleCollisions();
    }

    private void HandleCollisions()
    {
        if (_characterCollisionHandler.IsCollisionedAbove() && _velocity.y > 0f)
        {
            _velocity.y = 0f;
        }

        if (_isImpulseActive && _characterCollisionHandler.IsCollisionedBySide())
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