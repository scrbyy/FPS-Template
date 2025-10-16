using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airControl;
    [SerializeField] private InputProvider selectedInputProvider;

    private CharacterController _characterController;
    private Vector3 horizontalVelocityBuffer;
    private float verticalVector;

    private Vector3 moveVector;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void SetMoveSpeed(float newSpeed)
    {
        if(newSpeed > 0)
        {
            moveSpeed = newSpeed;
        }
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    private void Update()
    {
        UpdateVerticalVelocity();
        MovePlayer(selectedInputProvider.GetKeyboardInput());
    }

    private void MovePlayer(Vector2 keyboardInput)
    {
        Vector3 inputDirection = new Vector3(keyboardInput.x, 0, keyboardInput.y);

        if (inputDirection != Vector3.zero)
        {
            Vector3 desiredDirection = transform.TransformDirection(inputDirection.normalized);

            float controlMultiplier = _characterController.isGrounded ? 1f : airControl;

            Vector3 desiredVelocity = desiredDirection * moveSpeed;

            horizontalVelocityBuffer = Vector3.Lerp(
                horizontalVelocityBuffer,
                desiredVelocity,
                controlMultiplier * Time.deltaTime * 10f
            );
        }
        else if (_characterController.isGrounded)
        {
            horizontalVelocityBuffer = Vector3.Lerp(horizontalVelocityBuffer, Vector3.zero, Time.deltaTime * 10f);
        }
        moveVector = new Vector3(horizontalVelocityBuffer.x, verticalVector, horizontalVelocityBuffer.z);
        _characterController.Move(moveVector * Time.deltaTime);
    }


    private void UpdateVerticalVelocity()
    {
        if (_characterController.isGrounded)
        {
            verticalVector = -2f;

            if (selectedInputProvider.isJumpButtonDown())
            {
                verticalVector = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);

                if (selectedInputProvider.GetKeyboardInput() != Vector2.zero)
                {
                    Vector3 currentInputDirection = new Vector3(selectedInputProvider.GetKeyboardInput().x, 0, selectedInputProvider.GetKeyboardInput().y);
                    horizontalVelocityBuffer = transform.TransformDirection(currentInputDirection) * moveSpeed;
                }
            }
        }
        else
        {
            verticalVector += Physics.gravity.y * Time.deltaTime;
        }
    }
}