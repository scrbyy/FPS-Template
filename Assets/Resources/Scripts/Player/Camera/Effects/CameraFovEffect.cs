using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFovEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEngine playerEngine;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("FOV Limits")]
    [SerializeField] private float minSpeedThreshold = 5f;  
    [SerializeField] private float maxSpeedThreshold = 20f;

    [Space]
    [SerializeField] private float maxFovAdd = 12f;      
    [SerializeField] private float lerpSpeed = 7f;         
    [SerializeField] private float surgeSpeed = 15f;       

    private Camera _camera;
    private float _defaultFov;
    private float _currentFovVelocity;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _defaultFov = _camera.fieldOfView;
    }

    private void LateUpdate()
    {
        if (playerEngine == null) return;

        // 1. Получаем реальную горизонтальную скорость из Engine
        Vector3 velocity = playerEngine.GetVelocity();
        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        // 2. Рассчитываем фактор скорости (от 0 до 1)
        // 0 = идем медленнее порога, 1 = летим на максималках
        float speedFactor = Mathf.InverseLerp(minSpeedThreshold, maxSpeedThreshold, horizontalSpeed);

        // 3. Вычисляем целевой FOV
        float targetFov = _defaultFov + (speedFactor * maxFovAdd);

        // 4. Логика выбора скорости сглаживания
        // Если целевой FOV больше текущего — мы ускоряемся (нужна резкость)
        // Если меньше — замедляемся (нужна мягкость)
        float currentLerp = (targetFov > _camera.fieldOfView) ? surgeSpeed : lerpSpeed;

        // 5. Применяем изменения
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFov, Time.deltaTime * currentLerp);
    }
}