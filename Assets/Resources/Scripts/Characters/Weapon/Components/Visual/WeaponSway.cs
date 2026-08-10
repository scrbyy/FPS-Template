using UnityEngine;
using Zenject;

public class WeaponSway : RotationEffect
{
    [Header("Force Settings")]
    [SerializeField] private Vector2 _force;
    [SerializeField, Min(0f)] private float _smoothness;

    [Header("Invert")]
    [SerializeField] private bool _inverseX;
    [SerializeField] private bool _inverseY;

    [Header("Clamp X")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    [Header("Clamp Y")]
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    [Inject] private ILookInputProvider _inputProvider;

    private Quaternion _currentRotationOffset;

    private void Update()
    {
        CalculateSwayOffset();
    }

    private void CalculateSwayOffset()
    {
        var deltaTime = Time.deltaTime;
        var lookInput = _inputProvider.LookInput;

        var inverseX = _inverseX ? -1f : 1f;
        var inverseY = _inverseY ? -1f : 1f;

        var inputX = lookInput.x * inverseX;
        var inputY = lookInput.y * inverseY;

        float targetAngleX = Mathf.Clamp(inputY * _force.y, _minX, _maxX);
        float targetAngleY = Mathf.Clamp(inputX * _force.x, _minY, _maxY);

        Quaternion targetRotation = Quaternion.Euler(targetAngleX, targetAngleY, 0f);

        _currentRotationOffset = Quaternion.Slerp(
            _currentRotationOffset,
            targetRotation,
            deltaTime * _smoothness
        );
    }

    public override Quaternion GetLocalRotationOffset()
    {
        return _currentRotationOffset;
    }
}