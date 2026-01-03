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
        _playerVerticalPhysics.OnLanded -= ResetMaxSpeed;
        if (_characterController.isGrounded)
            _targetMaxSpeed = _walkSpeed;
        else
        {
            _playerVerticalPhysics.OnLanded += ResetMaxSpeed;
        }
    }

    private void Update()
    {
        maxSpeed = Mathf.MoveTowards(maxSpeed, _targetMaxSpeed, decelerationRate * Time.deltaTime);

        _playerVerticalPhysics.ApplyVerticalMovement();

        MovePlayer(selectedInputProvider.GetMoveVector());
    }

    private void MovePlayer(Vector2 keyboardInput)
    {
        Vector3 inputDir = new Vector3(keyboardInput.x, 0, keyboardInput.y).normalized;
        Vector3 wishDir = transform.TransformDirection(inputDir);

        if (_characterController.isGrounded)
        {
            if (inputDir.magnitude > 0)
            {
                _horizontalVelocityBuffer = Vector3.MoveTowards(
                    _horizontalVelocityBuffer,
                    wishDir * maxSpeed,
                    accelerationRate * Time.deltaTime);
            }
            else
            {
                _horizontalVelocityBuffer = Vector3.MoveTowards(
                    _horizontalVelocityBuffer,
                    Vector3.zero,
                    decelerationRate * Time.deltaTime);
            }
        }
        else
        {
            if (inputDir.magnitude > 0)
            {
                float currentSpeedInWishDir = Vector3.Dot(_horizontalVelocityBuffer, wishDir);
                float addSpeed = maxSpeed - currentSpeedInWishDir;

                if (addSpeed > 0)
                {
                    float accelSpeed = airControl * accelerationRate * Time.deltaTime;
                    accelSpeed = Mathf.Min(accelSpeed, addSpeed);

                    _horizontalVelocityBuffer += wishDir * accelSpeed;
                }
            }
        }
        _moveVector = new Vector3(_horizontalVelocityBuffer.x, _playerVerticalPhysics.GetVerticalVelocity(), _horizontalVelocityBuffer.z);
        _characterController.Move(_moveVector * Time.deltaTime);
    }
}