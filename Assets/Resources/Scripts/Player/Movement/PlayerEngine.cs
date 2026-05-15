using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerEngine : MonoBehaviour
{
    public event System.Action OnLanded;
    public event System.Action OnLeftGround;

    [Header("Movement")]
    [SerializeField] private float accelerationRate;
    [SerializeField] private float decelerationRate;

    [Header("Air Movement")]
    [SerializeField] private float airAcceleration;
    [SerializeField] private float airCap;

    [Header("Gravity")]
    [SerializeField] private float _gravityScale;
    [SerializeField] private float _downforce;

    private Vector3 _velocity;
    private CharacterController _characterController;

    private bool _wasGrounded;
    private bool _isImpulseActive;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector3 inputVector, float maxSpeed)
    {
        if (_characterController.isGrounded) ApplyGroundMovement(inputVector, maxSpeed);
        else ApplyAirMovement(inputVector);

        Vector3 finalMotion = _velocity;
        finalMotion.y = _velocity.y;

        _characterController.Move(finalMotion * Time.deltaTime);
    }

    public bool IsMoving(float threshold = 0.1f)
    {
        Vector3 horizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
        return horizontalVelocity.magnitude > threshold;
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

    public Vector3 GetVelocity() => _velocity;

    public bool isGrounded() => _characterController.isGrounded;

    public bool IsImpulseActive() => _isImpulseActive;

    private void Update()
    {
        DetectGroundTransitions();
        ApplyGravity();
        HandleHeadHit();
    }

    private void DetectGroundTransitions()
    {
        bool grounded = _characterController.isGrounded;
        if (grounded && !_wasGrounded) OnLanded?.Invoke();
        else if (!grounded && _wasGrounded) OnLeftGround?.Invoke();
        _wasGrounded = grounded;
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