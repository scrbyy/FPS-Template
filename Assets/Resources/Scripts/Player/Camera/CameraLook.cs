using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraLook : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float sensitivity;

    [Header("Clamping")]
    [SerializeField] private float maxAngle;
    [SerializeField] private float minAngle;

    [Header("References")]
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private InputProvider _inputProvider;

    private float _yRotation;
    private float _xRotation;

    private void Update()
    {
        float mouseY = _inputProvider.GetLookInput().x * sensitivity;
        float mouseX = _inputProvider.GetLookInput().y * sensitivity;

        _yRotation -= mouseX;
        _yRotation = Mathf.Clamp(_yRotation, minAngle, maxAngle);

        _xRotation += mouseY;
        _playerTransform.localRotation = Quaternion.Euler(0, _xRotation, 0);
        _headTransform.localRotation = Quaternion.Euler(_yRotation, 0, 0);
    }
}