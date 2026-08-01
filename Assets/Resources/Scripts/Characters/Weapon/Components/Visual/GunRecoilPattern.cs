using UnityEngine;

public class GunRecoilPattern : MonoBehaviour, IRotationEffect
{
    [SerializeField] private Vector2[] _recoilPattern;
    [SerializeField] private float _recoilMultiplier;
    [SerializeField] private float _snappiness; 
    [SerializeField] private float _returnSpeed;

    [SerializeField] private Gun _weapon;

    private int _currentShotIndex = 0;
    private Vector3 _targetRotation;
    private Vector3 _currentRotation;

    private void OnEnable()
    {
        if (_weapon != null)
        {
            _weapon.OnAttack += Fire;
            _weapon.OnStopAttack += StopFiring;
        }
    }

    private void OnDisable()
    {
        if (_weapon != null)
        {
            _weapon.OnAttack -= Fire;
            _weapon.OnStopAttack -= StopFiring;
        }
    }

    private void Update()
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);

        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _snappiness * Time.deltaTime);
    }

    private void Fire()
    {
        if (_recoilPattern == null || _recoilPattern.Length == 0) return;

        Vector2 patternOffset = _recoilPattern[_currentShotIndex] * _recoilMultiplier;

        _targetRotation += new Vector3(-patternOffset.y, patternOffset.x, 0f);

        _currentShotIndex = Mathf.Clamp(_currentShotIndex + 1, 0, _recoilPattern.Length - 1);
    }

    private void StopFiring()
    {
        _currentShotIndex = 0;
    }

    public Quaternion GetLocalRotationOffset()
    {
        return Quaternion.Euler(_currentRotation);
    }
}