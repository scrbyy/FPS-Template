using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraLook : MonoBehaviour
{
    [SerializeField] private float sensetivity;

    [SerializeField] private float maxCameraAngle;
    [SerializeField] private float minCameraAngle;

    [SerializeField] private Transform head;
    [SerializeField] private Transform player;

    [SerializeField] private InputProvider selectedInputProvider;

    private float _yRotation;
    private float _xRotation;
    private void Update()
    {
        float mouseY = selectedInputProvider.GetMouseInput().x * sensetivity;
        float mouseX = selectedInputProvider.GetMouseInput().y * sensetivity;

        _yRotation -= mouseX;
        _yRotation = Mathf.Clamp(_yRotation, minCameraAngle, maxCameraAngle);

        _xRotation += mouseY;
        player.localRotation = Quaternion.Euler(0, _xRotation, 0);
        head.localRotation = Quaternion.Euler(_yRotation, 0, 0);
    }
}