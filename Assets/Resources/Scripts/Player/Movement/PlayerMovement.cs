using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerJump))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelerationRate;
    [SerializeField] private float decelerationRate;
    [SerializeField] private float speedChangeRate;

    [Header("Air Control")]
    [SerializeField] private float airAcceleration;
    [SerializeField] private float airCap;

    [Header("Link Components")]
    [SerializeField] private InputProvider selectedInputProvider;

    private Vector3 _currentVelocity;
    private Vector3 _moveVector;
    private float _walkSpeed;
    private float _targetMaxSpeed;

    private CharacterController _characterController;
    private PlayerJump _playerVerticalPhysics;
    private bool _isImpulseActive;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerVerticalPhysics = GetComponent<PlayerJump>();

        _walkSpeed = maxSpeed;
        _targetMaxSpeed = maxSpeed;
    }

    public void SetMaxSpeed(float newMaxSpeed)
    {
        if (newMaxSpeed > 0)
        {
            _targetMaxSpeed = newMaxSpeed;
        }
    }

    public void ResetMaxSpeed()
    {
        _playerVerticalPhysics.OnLanded -= ResetMaxSpeed;
        if (_characterController.isGrounded)
            _targetMaxSpeed = _walkSpeed;
        else
            _playerVerticalPhysics.OnLanded += ResetMaxSpeed;
    }

    private void ApplyGroundMovement(Vector3 wishDir)
    {
        float targetSpeed = wishDir.magnitude * maxSpeed;

        if (targetSpeed > 0.01f)
        {
            _isImpulseActive = false;
        }

        if (_isImpulseActive)
        {
            // Пока импульс активен и нет ввода, мы плавно тормозим, 
            // но не ограничиваем скорость жестко через MoveTowards
            _currentVelocity = Vector3.MoveTowards(_currentVelocity, Vector3.zero, decelerationRate * Time.deltaTime);

            // Если скорость упала до нормальной, выключаем режим
            if (_currentVelocity.magnitude <= maxSpeed) _isImpulseActive = false;
            return;
        }

        // Стандартная логика движения
        float accel = (targetSpeed > 0 ? accelerationRate : decelerationRate);
        _currentVelocity = Vector3.MoveTowards(
          _currentVelocity,
          wishDir * targetSpeed,
          accel * Time.deltaTime
        );
    }

    public void AddForce(Vector3 force)
    {
        _currentVelocity += force;
        _isImpulseActive = true;
    }

    private void Update()
    {
        UpdateMaxSpeedState();
        ProcessMovement();
    }

    private void UpdateMaxSpeedState()
    {
        float currentRate = (maxSpeed < _targetMaxSpeed) ? speedChangeRate : decelerationRate;
        maxSpeed = Mathf.MoveTowards(maxSpeed, _targetMaxSpeed, currentRate * Time.deltaTime);
    }

    private void ProcessMovement()
    {
        Vector2 input = selectedInputProvider.GetMoveVector();
        Vector3 wishDir = transform.TransformDirection(new Vector3(input.x, 0, input.y)).normalized;

        if (_characterController.isGrounded)
            ApplyGroundMovement(wishDir);
        else
            ApplyAirMovement(wishDir);

        _moveVector = _currentVelocity;
        _moveVector.y = _playerVerticalPhysics.GetVerticalVelocity();

        _characterController.Move(_moveVector * Time.deltaTime);
    }

    private void ApplyAirMovement(Vector3 wishDir)
    {
        if (wishDir.magnitude == 0) return;

        float currentSpeedInWishDir = Vector3.Dot(_currentVelocity, wishDir);
        float addSpeed = Mathf.Max(0, airCap - currentSpeedInWishDir);

        if (addSpeed > 0)
        {
            float accelSpeed = Mathf.Min(airAcceleration * Time.deltaTime, addSpeed);
            _currentVelocity += wishDir * accelSpeed;
        }
    }
}