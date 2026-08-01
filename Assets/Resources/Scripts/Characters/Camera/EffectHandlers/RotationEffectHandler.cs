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

        foreach (GameObject effectObj in _effectList)
        {
            if (effectObj != null && effectObj.TryGetComponent<IRotationEffect>(out var effect))
            {
                _effects.Add(effect);
            }
        }

        _effects.AddRange(GetComponents<IRotationEffect>());
    }

    private void LateUpdate()
    {
        Vector3 totalEulerOffset = Vector3.zero;

        foreach (var effect in _effects)
        {
            if (effect != null)
            {
                Quaternion effectRotation = effect.GetLocalRotationOffset();

                totalEulerOffset += NormalizeAngles(effectRotation.eulerAngles);
            }
        }

        totalEulerOffset.z = 0f;

        transform.localRotation = _initialLocalRotation * Quaternion.Euler(totalEulerOffset);
    }

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