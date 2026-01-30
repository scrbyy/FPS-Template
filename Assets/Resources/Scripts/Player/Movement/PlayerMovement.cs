using UnityEngine;

[RequireComponent(typeof(PlayerEngine))]
[RequireComponent(typeof(PlayerJump))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float speedChangeRate;
    [SerializeField] private float decelerationRate;

    [Header("References")]
    [SerializeField] private InputProvider inputProvider;

    private PlayerEngine _motor;
    private PlayerJump _jump;
    private float _currentMaxSpeed;
    private float _targetMaxSpeed;

    private void Awake()
    {
        _motor = GetComponent<PlayerEngine>();
        _jump = GetComponent<PlayerJump>();
        _currentMaxSpeed = walkSpeed;
        _targetMaxSpeed = walkSpeed;
    }

    private void Update()
    {
        HandleSpeedTransition();

        Vector2 input = inputProvider.GetMoveVector();
        Vector3 wishDir = transform.TransformDirection(new Vector3(input.x, 0, input.y)).normalized;

        _motor.Move(wishDir, _currentMaxSpeed, _jump.GetVerticalVelocity());
    }

    private void HandleSpeedTransition()
    {
        float rate = (_currentMaxSpeed < _targetMaxSpeed) ? speedChangeRate : decelerationRate;
        _currentMaxSpeed = Mathf.MoveTowards(_currentMaxSpeed, _targetMaxSpeed, rate * Time.deltaTime);
    }

    public void SetTargetSpeed(float speed) => _targetMaxSpeed = speed;

    public void ResetSpeed()
    {
        if (GetComponent<CharacterController>().isGrounded)
            _targetMaxSpeed = walkSpeed;
        else
            _jump.OnLanded += OnLandedReset;
    }

    private void OnLandedReset()
    {
        _jump.OnLanded -= OnLandedReset;
        _targetMaxSpeed = walkSpeed;
    }
}