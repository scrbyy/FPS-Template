using UnityEngine;

[RequireComponent(typeof(PlayerEngine))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float speedChangeRate;
    [SerializeField] private float decelerationRate;

    [Header("References")]
    [SerializeField] private InputProvider inputProvider;
    [SerializeField] private PlayerJump playerJump;

    private PlayerEngine _playerEngine;
    private float _currentMaxSpeed;
    private float _targetMaxSpeed;

    private void Awake()
    {
        _playerEngine = GetComponent<PlayerEngine>();
        _currentMaxSpeed = walkSpeed;
        _targetMaxSpeed = walkSpeed;
    }

    private void Update()
    {
        HandleSpeedTransition();

        Vector2 input = inputProvider.GetMoveVector();
        Vector3 wishDir = transform.TransformDirection(new Vector3(input.x, 0, input.y)).normalized;

        _playerEngine.Move(wishDir, _currentMaxSpeed, playerJump.GetVerticalVelocity());
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
        else if (playerJump != null)
            playerJump.OnLanded += OnLandedReset;
    }
    public float GetCurrentMaxSpeed() => _currentMaxSpeed;
    public float GetTargetMaxSpeed() => _targetMaxSpeed;

    private void OnLandedReset()
    {
        playerJump.OnLanded -= OnLandedReset;
        _targetMaxSpeed = walkSpeed;
    }
}