using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEngine playerEngine;
    [SerializeField] private InputProvider inputProvider;

    [Header("Bobbing Settings")]
    [SerializeField] private AnimationCurve bobCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float verticalAmplitude = 0.04f;
    [SerializeField] private float horizontalAmplitude = 0.02f;
    [SerializeField] private float baseFrequency = 2.5f;
    [SerializeField] private float frequencyStiffness = 0.5f;

    [Header("Tilt Settings (Z-Axis)")]
    [SerializeField] private float tiltAmplitude = 1.5f; // Наклон при стрейфе
    [SerializeField] private float dashTiltMultiplier = 2.5f; // Усиление наклона при рывке
    [SerializeField] private float tiltSmoothSpeed = 8f;

    bool isDashing;

    private float _timer;
    private float _currentTiltZ;
    private Vector3 _initialPosition;

    private void Awake()
    {
        _initialPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        Vector3 velocity = playerEngine.GetVelocity();
        float horizontalSpeed = new Vector2(velocity.x, velocity.z).magnitude;

        // 1. Проверяем состояние рывка напрямую из Engine
        bool isDashing = playerEngine.IsImpulseActive(); // Замени на точное название твоего метода

        // 2. Логика перемещения камеры (Bobbing)
        if (horizontalSpeed > 0.2f && !isDashing)
        {
            // Обычное движение: считаем шаги
            float dynamicFrequency = baseFrequency + (Mathf.Sqrt(horizontalSpeed) * frequencyStiffness);
            _timer += Time.deltaTime * dynamicFrequency;

            HandleBobbing(horizontalSpeed);
        }
        else if (isDashing)
        {
            // Состояние рывка: плавно возвращаем камеру в центр (стабилизация взгляда)
            // Скорость возврата 10f дает приятный упругий эффект
            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition, Time.deltaTime * 10f);
        }
        else
        {
            // Покой: сброс таймера и позиции
            ResetMotion();
        }

        // 3. Наклон (Tilt): оставляем всегда, так как он дает физичность рывку
        HandleTilt(velocity);
    }

    private void HandleBobbing(float speed)
    {
        float waveX = Mathf.Sin(_timer * 0.5f);
        float waveY = Mathf.Sin(_timer);

        float curveX = bobCurve.Evaluate((waveX + 1f) * 0.5f) * 2f - 1f;
        float curveY = bobCurve.Evaluate((waveY + 1f) * 0.5f) * 2f - 1f;

        // Амплитуда чуть растет со скоростью
        float ampFactor = Mathf.Clamp(speed / 7f, 0.7f, 1.4f);

        transform.localPosition = _initialPosition + new Vector3(
            curveX * horizontalAmplitude * ampFactor,
            curveY * verticalAmplitude * ampFactor,
            0
        );
    }

    private void HandleTilt(Vector3 velocity)
    {
        // 1. Рассчитываем локальную скорость для определения направления наклона
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float lateralSpeed = localVelocity.x;

        // 2. Вычисляем целевой угол наклона по оси Z
        float targetTilt = -lateralSpeed * (tiltAmplitude / 5f);
        targetTilt = Mathf.Clamp(targetTilt, -tiltAmplitude * dashTiltMultiplier, tiltAmplitude * dashTiltMultiplier);

        // 3. Сглаживаем само значение угла
        _currentTiltZ = Mathf.Lerp(_currentTiltZ, targetTilt, Time.deltaTime * tiltSmoothSpeed);

        // 4. ПРИМЕНЕНИЕ БЕЗ КОНФЛИКТА С МЫШКОЙ:
        // Создаем поворот только вокруг оси Forward (Z)
        Quaternion tiltRotation = Quaternion.AngleAxis(_currentTiltZ, Vector3.forward);

        // Умножаем ТЕКУЩЕЕ вращение камеры (от мыши) на наклон Z.
        // Это добавит наклон к любому углу, куда смотрит игрок.
        transform.localRotation = transform.localRotation * tiltRotation;

        // ВАЖНО: Если после этого камера все еще "замерзает", значит твой скрипт мыши 
        // работает в LateUpdate одновременно с этим. 
        // Убедись, что MouseLook в Update(), а Bobbing в LateUpdate().
    }

    private void ResetMotion()
    {
        _timer = 0;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition, Time.deltaTime * 5f);

        // Плавный возврат наклона в 0
        _currentTiltZ = Mathf.Lerp(_currentTiltZ, 0, Time.deltaTime * 5f);

        // Применяем возврат к 0, не сбрасывая X и Y
        Quaternion tiltRotation = Quaternion.AngleAxis(_currentTiltZ, Vector3.forward);
        // Это деликатно возвращает Z в ноль относительно текущего взгляда
        transform.localRotation = transform.localRotation * tiltRotation;
    }
}