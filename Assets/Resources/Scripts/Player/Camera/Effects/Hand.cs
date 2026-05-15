using UnityEngine;
using Zenject;

public class WeaponVelocityEffect : MonoBehaviour, IMotionEffect
{
    [Header("Limits (Z Offset)")]
    [SerializeField] private float _maxZOffsetChange;

    [Header("Changing Rate")]
    [SerializeField] private float _increaseSpeed;
    [SerializeField] private float _decreaseSpeed;

    [Header("Speed Thresholds")]
    [SerializeField] private float _minSpeedThreshold;
    [SerializeField] private float _maxSpeedThreshold;

    [Header("References")]
    [SerializeField] private PlayerEngine _playerEngine;
    [Inject] private IInputProvider _inputProvider;

    private float _currentZOffset;
    private float targetZOffset;

    public Vector3 GetLocalOffset()
    {
        return new Vector3(0, 0, _currentZOffset);
    }

    private void LateUpdate()
    {
        if (_playerEngine == null) return;

        Vector3 velocity = _playerEngine.GetVelocity();
        Vector3 direction = velocity.normalized;

        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        Transform playerTransform = _playerEngine.transform;

        float dot = Vector3.Dot(new Vector3(0.5f, 0, 1), direction);
        float directionModifier = _inputProvider.GetMoveVector().y != 0 ? 1 : 0;

        float modifier = Mathf.InverseLerp(_minSpeedThreshold, _maxSpeedThreshold, horizontalSpeed);

        targetZOffset = 0 + (modifier * _maxZOffsetChange) * directionModifier;

        float currentLerp = (Mathf.Abs(targetZOffset) > Mathf.Abs(_currentZOffset)) ? _increaseSpeed : _decreaseSpeed;
        _currentZOffset = Mathf.Lerp(_currentZOffset, targetZOffset, Time.deltaTime * currentLerp);

        Vector3 localPos = transform.localPosition;
        transform.localPosition = new Vector3(localPos.x, localPos.y, _currentZOffset);
    }
}