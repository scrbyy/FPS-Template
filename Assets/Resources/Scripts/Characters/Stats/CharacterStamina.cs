using UnityEngine;
using System.Collections;

public class CharacterStamina : CharacterStat
{
    [Header("Recovery Settings")]
    [SerializeField] private float _recoveryCooldown;
    [SerializeField] private float _recoverySpeed;

    private WaitForSeconds _cooldown;
    private Coroutine _recoveryCoroutine;
    private bool _isExhausted;

    public override void Decrease(float reducingValue)
    {
        if (_isExhausted) return;

        if (reducingValue > 0)
        {
            _currentValue = Mathf.Max(_currentValue - reducingValue, 0);

            NotifyValueChanged(_currentValue);

            if (_currentValue <= 0) _isExhausted = true;

            if (_recoveryCoroutine != null) StopCoroutine(_recoveryCoroutine);

            _recoveryCoroutine = StartCoroutine(RecoveryRoutine());
        }
    }

    public bool IsEnoughStamina(float amount)
    {
        return !_isExhausted && _currentValue >= amount;
    }

    private void Start()
    {
        _cooldown = new WaitForSeconds(_recoveryCooldown);
        NotifyValueChanged(_currentValue);
    }
    
    private IEnumerator RecoveryRoutine()
    {
        yield return _cooldown;

        while (_currentValue < MaxValue)
        {
            _currentValue = Mathf.MoveTowards(_currentValue, MaxValue, _recoverySpeed * Time.deltaTime);
            NotifyValueChanged(_currentValue);
            yield return null;
        }
        _isExhausted = false;
        _recoveryCoroutine = null;
    }
}