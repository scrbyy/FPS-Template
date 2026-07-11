using UnityEngine;
using Zenject;

public class TransformSway : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private Vector2 _force;
    [SerializeField, Min(0f)] private float _multiplier;
    [SerializeField] private bool _inverseX;
    [SerializeField] private bool _inverseY;

    [Header("Clamp X")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    [Header("Clamp Y")]
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    [Inject] private ILookInputProvider _inputProvider;

    private float _additionalX;
    private float _additionalY;

    private float _mouseX, _mouseY;

    private void LateUpdate()
    {
        PerformTransformSway();
    }

    private void PerformTransformSway()
    {
        var deltaTime = Time.deltaTime;
        var inverseSwayX = _inverseX ? -1f : 1f;
        var inverseSwayY = _inverseY ? -1f : 1f;

        _mouseX = _inputProvider.LookInput.x * inverseSwayX;
        _mouseY = _inputProvider.LookInput.y * inverseSwayY;

        var currentX = _mouseY * _force.y;
        var currentY = _mouseX * _force.x;

        var endEulerAngleX = Mathf.Clamp(currentX + _additionalX, _minX, _maxX);
        var endEulerAngleY = Mathf.Clamp(currentY + _additionalY, _minY, _maxY);

        var moment = deltaTime * _multiplier;
        var localEulerAngles = transform.localEulerAngles;

        localEulerAngles.x = Mathf.LerpAngle(localEulerAngles.x, endEulerAngleX, moment);
        localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, endEulerAngleY, moment);
        localEulerAngles.z = 0f;

        transform.localEulerAngles = localEulerAngles;
    }
}