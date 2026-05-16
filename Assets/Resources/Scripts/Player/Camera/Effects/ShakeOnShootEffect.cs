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
    [SerializeField] private WeaponContainer _weaponContainer;

    private Vector3 _originalPosition;

    private void Shake()
    {
        transform.DOKill();
        transform.localPosition = _originalPosition;

        transform.DOShakePosition(_duration, _strength, _vibrato, _randomness)
            .OnComplete(() => transform.localPosition = _originalPosition);
    }

    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        _weaponContainer.GetCurrent().OnWeaponShoot += Shake;
    }

    private void OnDisable()
    {
        _weaponContainer.GetCurrent().OnWeaponShoot -= Shake;
    }
}