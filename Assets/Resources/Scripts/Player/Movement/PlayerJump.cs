using Zenject;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float staminaCost;

    [Header("Buffer")]
    [SerializeField] private float bufferDuration;

    [Header("References")]
    [SerializeField] private PlayerEngine _playerEngine;
    [SerializeField] private PlayerStamina playerStamina;

    [Inject] private IInputProvider inputProvider;

    private Buffer _inputBuffer;

    public void Jump(float jumpForce)
    {
        _inputBuffer.Set();
        if (_playerEngine.isGrounded() && playerStamina.IsEnoughStamina(staminaCost))
        {
            _playerEngine.AddForce(Vector3.up * jumpForce, ForceType.Jump);
            playerStamina.Decrease(staminaCost);
        }
    }

    private void Update()
    {
        if (_inputBuffer.Has() && _playerEngine.isGrounded())
        {
            Jump(jumpForce);
            _inputBuffer.Reset();
        }
    }

    private void Start()
    {
        _inputBuffer = new Buffer(bufferDuration);
    }

    private void JumpWithBuffer()
    {
        Jump(jumpForce);
    }

    private void OnEnable() => inputProvider.OnJumpPerformed += JumpWithBuffer;
    private void OnDisable() => inputProvider.OnJumpPerformed -= JumpWithBuffer;
}