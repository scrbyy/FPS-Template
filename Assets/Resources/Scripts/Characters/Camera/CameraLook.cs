using UnityEngine;
using Zenject;

[RequireComponent(typeof(Camera))]
public class CameraLook : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField] private float _sensitivity;

    [Header("Clamping")]
    [SerializeField] private float _maxViewAngle;
    [SerializeField] private float _minViewAngle;

    [Header("References")]
    [SerializeField] private Transform _headTransform;  
    [SerializeField] private Transform _playerTransform;
    [Inject] private ILookInputProvider _inputProvider;

    private float _yRotation; 
    private float _xRotation; 

    private void Update()
    {
        Vector2 lookInput = _inputProvider.LookInput;

        float mouseX = lookInput.x * _sensitivity;
        float mouseY = lookInput.y * _sensitivity;

        _xRotation += mouseX;
        _playerTransform.localRotation = Quaternion.Euler(0, _xRotation, 0);

        _yRotation -= mouseY;
        _yRotation = Mathf.Clamp(_yRotation, _minViewAngle, _maxViewAngle);

        _headTransform.localRotation = Quaternion.Euler(_yRotation, 0, _headTransform.localEulerAngles.z);
    }
}