using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerVerticalPhysics : MonoBehaviour
{
    public event System.Action OnLanded;
    public event System.Action OnLeftGround;

    public bool IsGrounded => _characterController.isGrounded;

    [SerializeField] private float jumpForce;
    [SerializeField] private float bufferDuration;
    [SerializeField] private float gravityScale;
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
    private void OnEnable()
    {
        inputProvider.OnJumpPerformed += JumpWithBuffer;
    }
    private void OnDisable()
    {
        inputProvider.OnJumpPerformed -= JumpWithBuffer;
    }

    private void Update()
    {
        if(_inputBuffer.HasBuffer() && _characterController.isGrounded)
        {
            Jump(jumpForce);
            _inputBuffer.ConsumeBuffer();
        }
            DetectGroundTransitions();
        ApplyGravity();
    }

    private void DetectGroundTransitions()
    {
        bool grounded = _characterController.isGrounded;

        if (grounded && !_wasGrounded)
            OnLanded?.Invoke();
        else if (!grounded && _wasGrounded)
            OnLeftGround?.Invoke();

        _wasGrounded = grounded;
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;
        else
            _verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
    }

    public void Jump(float jumpForce)
    {
        if (_characterController.isGrounded)
            _verticalVelocity = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
    }
    public float GetVerticalVelocity()
    {
        return _verticalVelocity;
    }

    public void ApplyVerticalMovement()
    {
        Vector3 verticalMotion = new Vector3(0, _verticalVelocity, 0) * Time.deltaTime;

        CollisionFlags flags = _characterController.Move(verticalMotion);

        if ((flags & CollisionFlags.Above) != 0 && _verticalVelocity > 0f)
        {
            _verticalVelocity = 0f;
        }
    }
    private void JumpWithBuffer()
    {
        _inputBuffer.SetBuffer();
        Jump(jumpForce);
    }
}
