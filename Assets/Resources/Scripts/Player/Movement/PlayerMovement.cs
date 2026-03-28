using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerEngine))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private float _walkSpeed;

    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;

    [Header("References")]
    [Inject] private IInputProvider _inputProvider;

    private float _targetMaxSpeed;
    private float _currentMaxSpeed;

    private PlayerEngine _playerEngine;

    private void Awake()
    {
        _playerEngine = GetComponent<PlayerEngine>();

        _currentMaxSpeed = _walkSpeed;
        _targetMaxSpeed = _walkSpeed;
    }

    private void Update()
    {
        HandleSpeedTransition();

        Vector2 input = _inputProvider.GetMoveVector();
        Vector3 wishDir = transform.TransformDirection(new Vector3(input.x, 0, input.y)).normalized;

        _playerEngine.Move(wishDir, _currentMaxSpeed);
    }

    private void HandleSpeedTransition()
    {
        float rate = (_currentMaxSpeed < _targetMaxSpeed) ? _acceleration : _deceleration;
        _currentMaxSpeed = Mathf.MoveTowards(_currentMaxSpeed, _targetMaxSpeed, rate * Time.deltaTime);
    }

    public void SetTargetSpeed(float speed) => _targetMaxSpeed = speed;

    public void ResetSpeed()
    {
        if (_playerEngine.isGrounded())
            _targetMaxSpeed = _walkSpeed;
        else _playerEngine.OnLanded += OnLandedReset;
    }
    public float GetCurrentMaxSpeed() => _currentMaxSpeed;
    public float GetTargetMaxSpeed() => _targetMaxSpeed;

    private void OnLandedReset()
    {
        _playerEngine.OnLanded -= OnLandedReset;
        _targetMaxSpeed = _walkSpeed;
    }
}