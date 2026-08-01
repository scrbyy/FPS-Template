using System.Collections.Generic;
using UnityEngine;

public class RotationEffectHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _effectList = new List<GameObject>();

    private List<IRotationEffect> _effects = new List<IRotationEffect>();
    private Quaternion _initialLocalRotation;

    private void Awake()
    {
        _initialLocalRotation = transform.localRotation;

        // 1. Собираем эффекты из внешних GameObject'ов
        foreach (GameObject effectObj in _effectList)
        {
            if (effectObj != null && effectObj.TryGetComponent<IRotationEffect>(out var effect))
            {
                _effects.Add(effect);
            }
        }

        // 2. Добавляем эффекты с этого же GameObject
        _effects.AddRange(GetComponents<IRotationEffect>());
    }

    private void LateUpdate()
    {
        Vector3 totalEulerOffset = Vector3.zero;

        // ИСПРАВЛЕНО: Складываем углы Эйлера напрямую, а не перемножаем кватернионы
        foreach (var effect in _effects)
        {
            if (effect != null)
            {
                // Получаем офсет и берем его углы Эйлера
                Quaternion effectRotation = effect.GetLocalRotationOffset();

                // Чтобы избежать проблем с gimbal lock и скачками при углах > 180,
                // используем normalizedEulerAngles, если они доступны,
                // или приводим углы к диапазону -180..180 вручную.
                totalEulerOffset += NormalizeAngles(effectRotation.eulerAngles);
            }
        }

        // Жестко фиксируем Z на всякий случай (хотя в нормированных углах X и Y он будет 0)
        totalEulerOffset.z = 0f;

        // Применяем начальное вращение, умноженное на один итоговый кватернион
        transform.localRotation = _initialLocalRotation * Quaternion.Euler(totalEulerOffset);
    }

    // Вспомогательная функция для нормировки углов в диапазон -180..180
    private Vector3 NormalizeAngles(Vector3 angles)
    {
        angles.x = NormalizeAngle(angles.x);
        angles.y = NormalizeAngle(angles.y);
        angles.z = NormalizeAngle(angles.z);
        return angles;
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}