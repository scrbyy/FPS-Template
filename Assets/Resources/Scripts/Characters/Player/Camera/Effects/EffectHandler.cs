using UnityEngine;
using System.Collections.Generic;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private bool _useSmoothing;
    [SerializeField] private float _smoothSpeed;

    private List<IMotionEffect> _effects = new List<IMotionEffect>();

    private Vector3 _targetOffset;

    private Vector3 _initialLocalPosition;

    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;

        _effects.AddRange(GetComponents<IMotionEffect>());
    }

    private void LateUpdate()
    {
        Vector3 totalOffset = Vector3.zero;

        for (int i = 0; i < _effects.Count; i++)
        {
            totalOffset += _effects[i].GetLocalOffset();
        }

        if (_useSmoothing)
        {
            _targetOffset = Vector3.Lerp(_targetOffset, totalOffset, Time.deltaTime * _smoothSpeed);
        }
        else
        {
            _targetOffset = totalOffset;
        }

        transform.localPosition = _initialLocalPosition + _targetOffset;
    }
}