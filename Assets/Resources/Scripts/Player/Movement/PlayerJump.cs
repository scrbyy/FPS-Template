using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : MonoBehaviour
{
    public event System.Action OnLanded;
    public event System.Action OnLeftGround;

    [Header("Main")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;
    [SerializeField] private float groundedGravity;

    [Header("Stamina")]
    [SerializeField] private float staminaCost;

    [Header("Input Buffer")]
    [SerializeField] private float bufferDuration;

    [Header("Link Components")]
    [SerializeField] private PlayerStamina playerStamina;
    [SerializeField] private InputProvider inputProvider;

    private bool _wasGrounded;
    private float _verticalVelocity;
    private InputBuffer _inputBuffer;
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _inputBuffer = new InputBuffer(bufferDuration);
    }

    private void OnEnable() => inputProvider.OnJumpPerformed += JumpWithBuffer;
    private void OnDisable() => inputProvider.OnJumpPerformed -= JumpWithBuffer;

    private void Update()
    {
        if (_inputBuffer.HasBuffer() && _characterController.isGrounded)
        {
            Jump(jumpForce);
            _inputBuffer.ConsumeBuffer();
        }

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
        if (_characterController.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = groundedGravity;
        else
            _verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
    }

    private void HandleHeadHit()
    {
        if ((_characterController.collisionFlags & CollisionFlags.Above) != 0 && _verticalVelocity > 0f)
        {
            _verticalVelocity = 0f;
        }
    }

    public void Jump(float jumpForce)
    {
        if (_characterController.isGrounded && playerStamina.IsEnoughStamina(staminaCost))
        {
            _verticalVelocity = Mathf.Sqrt(jumpForce * groundedGravity * Physics.gravity.y);
            playerStamina.ReduceStamina(staminaCost);
        }
    }

    public float GetVerticalVelocity() => _verticalVelocity;

    private void JumpWithBuffer()
    {
        _inputBuffer.SetBuffer();
        Jump(jumpForce);
    }

    internal void ResetVerticalVelocity()
    {
        _verticalVelocity = groundedGravity;
    }
}
