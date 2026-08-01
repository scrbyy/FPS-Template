using UnityEngine;
using System.Collections.Generic;

public class PositionEffectHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _effectList = new List<GameObject>();
    private List<IPositionEffect> _effects = new List<IPositionEffect>();

    private Vector3 _targetOffset;
    private Vector3 _initialLocalPosition;

    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;

        foreach (GameObject effectObj in _effectList)
        {
            if (effectObj != null && effectObj.TryGetComponent<IPositionEffect>(out var effect))
            {
                _effects.Add(effect);
            }
        }

        _effects.AddRange(GetComponents<IPositionEffect>());
    }

    private void LateUpdate()
    {
        Vector3 totalOffset = Vector3.zero;

        foreach (var effect in _effects)
        {
            totalOffset += effect.GetLocalOffset();
        }

        _targetOffset = totalOffset;

        transform.localPosition = _initialLocalPosition + _targetOffset;
    }
}