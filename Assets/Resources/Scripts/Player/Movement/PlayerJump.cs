using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float staminaCost;

    [Header("Buffers")]
    [SerializeField] private float bufferDuration;

    [Header("References")]
    [SerializeField] private PlayerStamina playerStamina;
    [SerializeField] private InputProvider inputProvider;

    private PlayerEngine _playerEngine;
    private Buffer _inputBuffer;

    private void Start()
    {
        _playerEngine = GetComponent<PlayerEngine>();   
        _inputBuffer = new Buffer(bufferDuration);
    }

    private void OnEnable() => inputProvider.OnJumpPerformed += JumpWithBuffer;
    private void OnDisable() => inputProvider.OnJumpPerformed -= JumpWithBuffer;

    private void Update()
    {
        if (_inputBuffer.Has() && _playerEngine.isGrounded())
        {
            Jump(jumpForce);
            _inputBuffer.Reset();
        }
    }

    public void Jump(float jumpForce)
    {
        if (_playerEngine.isGrounded() && playerStamina.IsEnoughStamina(staminaCost))
        {
            _playerEngine.AddForce(Vector3.up * jumpForce, ForceType.Jump);
            playerStamina.ReduceStamina(staminaCost);
        }
    }

    private void JumpWithBuffer()
    {
        _inputBuffer.Set();
        Jump(jumpForce);
    }
}