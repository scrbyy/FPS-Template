using Zenject;
using UnityEngine;

public class MovementApproachingEffect : MonoBehaviour, IPositionEffect
{
    [Header("Limits")]
    [SerializeField] private float _maxZOffset;

    [Header("Changing Rate")]
    [SerializeField] private float _increaseSpeed = 5f;
    [SerializeField] private float _decreaseSpeed = 2f;

    [Header("Speed Thresholds")]
    [SerializeField] private float _minSpeedThreshold;
    [SerializeField] private float _maxSpeedThreshold;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;
    [Inject] private IMovementInputProvider _inputProvider;

    private float _currentZOffset;
    private float _targetZOffset;

    public Vector3 GetLocalOffset()
    {   
        return new Vector3(0, 0, _currentZOffset);
    }

    private void LateUpdate()
    {
        Vector3 velocity = _characterEngine.Velocity;
        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        bool isMovingForwardOrBackward = !Mathf.Approximately(_inputProvider.MoveInput.y, 0f);
        float directionModifier = isMovingForwardOrBackward ? 1f : 0f;

        float modifier = Mathf.InverseLerp(_minSpeedThreshold, _maxSpeedThreshold, horizontalSpeed);

        _targetZOffset = (modifier * _maxZOffset) * directionModifier;

        bool isIncreasing = Mathf.Abs(_targetZOffset) > Mathf.Abs(_currentZOffset);
        float currentSpeed = isIncreasing ? _increaseSpeed : _decreaseSpeed;

        _currentZOffset = Mathf.MoveTowards(_currentZOffset, _targetZOffset, currentSpeed * Time.deltaTime);
    }
}