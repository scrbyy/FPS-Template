using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField] private InputProvider selectedInputProvider;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private CharacterController _characterController;
    private Vector3 horizontalVelocityBuffer;
    private float verticalVector;

    private Vector3 moveVector;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateVerticalVelocity();
        MovePlayer(selectedInputProvider.GetKeyboardInput());
    }

    private void MovePlayer(Vector2 keyboardInput)
    {
        Vector3 desiredMovement;
        Vector3 desiredDirection = new Vector3(keyboardInput.x, 0, keyboardInput.y);

        if (desiredDirection != Vector3.zero)
        {
            desiredMovement = transform.TransformDirection(desiredDirection) * moveSpeed;
            horizontalVelocityBuffer = desiredMovement;
            
        }
        else
        {
            desiredMovement = horizontalVelocityBuffer;

            if (_characterController.isGrounded)
            {
                horizontalVelocityBuffer = Vector3.Lerp(horizontalVelocityBuffer, Vector3.zero, Time.deltaTime * 10f);
            }
        }

        moveVector = new Vector3(desiredMovement.x, verticalVector, desiredMovement.z);
        _characterController.Move(moveVector * Time.deltaTime);
    }

    private void UpdateVerticalVelocity()
    {
        if (_characterController.isGrounded)
        {
            verticalVector = -2f;

            if (selectedInputProvider.isJumpButtonPressed())
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