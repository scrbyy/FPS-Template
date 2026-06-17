using UnityEngine;
using Zenject;

public class TransformSway : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private Vector2 _force;
    [SerializeField, Min(0f)] private float _multiplier;
    [SerializeField] private bool _inverseX;
    [SerializeField] private bool _inverseY;

    [Header("Clamp")]
    [SerializeField] private Vector2 _minMaxX;
    [SerializeField] private Vector2 _minMaxY;

    [Header("References")]
    [Inject] private ILookInputProvider _inputProvider;

    protected float AdditionalX;
    protected float AdditionalY;

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

        OnSwayPerforming(deltaTime);

        var currentX = _mouseY * _force.y;
        var currentY = _mouseX * _force.x;

        var endEulerAngleX = Mathf.Clamp(currentX + AdditionalX, _minMaxX.x, _minMaxX.y);
        var endEulerAngleY = Mathf.Clamp(currentY + AdditionalY, _minMaxY.x, _minMaxY.y);

        var moment = deltaTime * _multiplier;
        var localEulerAngles = transform.localEulerAngles;

        localEulerAngles.x = Mathf.LerpAngle(localEulerAngles.x, endEulerAngleX, moment);
        localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, endEulerAngleY, moment);
        localEulerAngles.z = 0f;

        transform.localEulerAngles = localEulerAngles;
    }

    protected virtual void OnSwayPerforming(in float deltaTime) { }
}