using UnityEngine;
using DG.Tweening;

public class ShakeOnShootEffect : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float _duration;    
    [SerializeField] private float _strength;   
    [SerializeField] private int _vibrato;         
    [SerializeField] private float _randomness;

    [Header("References")]
    [SerializeField] private Gun _gun;
    [SerializeField] private Transform _cameraTransform;

    private Vector3 _originalPosition;

    private void Shake()
    {
        _cameraTransform.DOKill();
        _cameraTransform.localPosition = _originalPosition;

        _cameraTransform.DOShakePosition(_duration, _strength, _vibrato, _randomness)
            .OnComplete(() => _cameraTransform.localPosition = _originalPosition);
    }

    private void Awake()
    {
        _originalPosition = _cameraTransform.localPosition;
    }

    private void OnEnable()
    {
        _gun.OnAttack += Shake;
    }

    private void OnDisable()
    {
        _gun.OnAttack -= Shake;
    }
}