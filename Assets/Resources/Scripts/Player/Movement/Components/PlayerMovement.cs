using Zenject;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;

    [SerializeField] private float _accelerationRate;
    [SerializeField] private float _decelerationRate;

    [Header("References")]
    [SerializeField] private PlayerEngine _playerEngine;

    [Inject] private IInputProvider _inputProvider;
    [Inject] private IGroundChecker _groundCheck;


    private float _targetSpeed;

    public void SetTargetSpeed(float speed) => _targetSpeed = speed;

    public void ResetSpeed()
    {
        if (_groundCheck.IsGrounded)
            _targetSpeed = _walkSpeed;
        else _groundCheck.OnGrounded += OnLandedReset;
    }

    private void Start()
    {
        _targetSpeed = _walkSpeed;
    }

    private void Update()
    {
        Vector2 input = _inputProvider.GetMoveVector();
        Vector3 wishDir = transform.TransformDirection(new Vector3(input.x, 0, input.y)).normalized;
        _playerEngine.Move(wishDir, _targetSpeed);
    }

    private void OnLandedReset()
    {
        _groundCheck.OnGrounded -= OnLandedReset;
        _targetSpeed = _walkSpeed;
    }
}