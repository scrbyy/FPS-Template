using UnityEngine;
using Zenject;

public class ApproachingEffect : MonoBehaviour, IMotionEffect
{
    [Header("Limits")]
    [SerializeField] private float _maxZOffset;

    [Header("Changing Rate")]
    [SerializeField] private float _increaseSpeed;
    [SerializeField] private float _decreaseSpeed;

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
        Vector3 direction = velocity.normalized;

        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        Transform playerTransform = _characterEngine.transform;

        float dot = Vector3.Dot(new Vector3(0.5f, 0, 1), direction);
        float directionModifier = _inputProvider.MoveInput.y != 0 ? 1 : 0;

        float modifier = Mathf.InverseLerp(_minSpeedThreshold, _maxSpeedThreshold, horizontalSpeed);

        _targetZOffset = (modifier * _maxZOffset) * directionModifier;

        float currentLerp = (Mathf.Abs(_targetZOffset) > Mathf.Abs(_currentZOffset)) ? _increaseSpeed : _decreaseSpeed;
        _currentZOffset = Mathf.Lerp(_currentZOffset, _targetZOffset, Time.deltaTime * currentLerp);
    }
}