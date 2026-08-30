using UnityEngine;
using System.Collections.Generic;

public class PositionEffectHandler : MonoBehaviour
{
    [SerializeField] private List<PositionEffect> _effects = new List<PositionEffect>();
     
    private Vector3 _targetOffset;
    private Vector3 _initialLocalPosition;

    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;
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