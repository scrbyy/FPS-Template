using Zenject;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _staminaCost;

    [Header("Buffer")]
    [SerializeField] private float _bufferDuration;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;
    [SerializeField] private CharacterStamina playerStamina;

    [Inject] private IInputProvider _inputProvider;
    [Inject] private IGroundChecker _groundCheck;

    private Buffer _inputBuffer;

    public void Jump(float jumpForce)
    {
        _inputBuffer.Set();
        if (_groundCheck.IsGrounded && playerStamina.IsEnoughStamina(_staminaCost))
        {
            _characterEngine.AddForce(Vector3.up * jumpForce, ForceType.Jump);
            playerStamina.Decrease(_staminaCost);
        }
    }

    private void Update()
    {
        if (_inputBuffer.Has() && _groundCheck.IsGrounded)
        {
            Jump(_jumpForce);
            _inputBuffer.Reset();
        }
    }

    private void Start()
    {
        _inputBuffer = new Buffer(_bufferDuration);
    }

    private void JumpWithBuffer()
    {
        Jump(_jumpForce);
    }

    private void OnEnable() => _inputProvider.OnJumpPerformed += JumpWithBuffer;
    private void OnDisable() => _inputProvider.OnJumpPerformed -= JumpWithBuffer;
}