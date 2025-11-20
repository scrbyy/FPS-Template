using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerVerticalPhysics))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelerationRate;
    [SerializeField] private float decelerationRate;
    [SerializeField] private float airControl;
    [SerializeField] private InputProvider selectedInputProvider;

    private Vector3 _horizontalVelocityBuffer;
    private Vector3 _moveVector;

    private float _walkSpeed;
    private float _targetMaxSpeed;

    private CharacterController _characterController;
    private PlayerVerticalPhysics _playerVerticalPhysics;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerVerticalPhysics = GetComponent<PlayerVerticalPhysics>();

        _walkSpeed = maxSpeed;
        _targetMaxSpeed = maxSpeed;
    }

    public void SetMaxSpeed(float newMaxSpeed)
    {
        if (newMaxSpeed > 0 && _characterController.isGrounded)
            _targetMaxSpeed = newMaxSpeed;
    }

    public void ResetMaxSpeed()
    {
        if(_characterController.isGrounded)
            _targetMaxSpeed = _walkSpeed;
    }

    private void Update()
    {
        maxSpeed = Mathf.MoveTowards(maxSpeed, _targetMaxSpeed, decelerationRate * Time.deltaTime);

        _playerVerticalPhysics.ApplyVerticalMovement();

        MovePlayer(selectedInputProvider.GetMoveVector());
    }

    private void MovePlayer(Vector2 keyboardInput)
    {
        Vector3 inputDirection = new Vector3(keyboardInput.x, 0, keyboardInput.y);

        Vector3 desiredDirection = transform.TransformDirection(inputDirection.normalized);
        Vector3 desiredVelocity = desiredDirection * maxSpeed;

        if (inputDirection != Vector3.zero)
        {
            float controlRate = _characterController.isGrounded ? accelerationRate : airControl * accelerationRate;

            _horizontalVelocityBuffer = Vector3.Lerp(
                _horizontalVelocityBuffer,
                desiredVelocity,
                controlRate * Time.deltaTime);
        }
        else
        {
            if (_characterController.isGrounded)
            {
                _horizontalVelocityBuffer = Vector3.Lerp(_horizontalVelocityBuffer, Vector3.zero, Time.deltaTime * decelerationRate);
            }
        }

        if (_horizontalVelocityBuffer.magnitude > maxSpeed)
        {
            _horizontalVelocityBuffer = _horizontalVelocityBuffer.normalized * maxSpeed;
        }

        _moveVector = new Vector3(_horizontalVelocityBuffer.x, _playerVerticalPhysics.GetVerticalVelocity(), _horizontalVelocityBuffer.z);
        _characterController.Move(_moveVector * Time.deltaTime);
    }
}