using UnityEngine;
using System.Collections;

public class PlayerStamina : MonoBehaviour
{
    public event System.Action<float> OnPlayerStaminaChanged;

    [Header("Main")]
    [SerializeField] private float currentStamina;
    [SerializeField] private float maxStamina;

    [Header("Recovery Settings")]
    [SerializeField] private float recoveryDelay = 2f;
    [SerializeField] private float recoverySpeed = 15f;

    [Header("Tick Settings")]
    [SerializeField] private float ticksPerSecond = 60f;
    private float _lastTickTime;

    private Coroutine _recoveryCoroutine;

    private void Start()
    {
        currentStamina = maxStamina;
        OnPlayerStaminaChanged?.Invoke(currentStamina);
    }

    private bool _isExhausted;

    public float GetMaxStamina()
    {
        return maxStamina;
    }
    public void ReduceStamina(float amount)
    {
        if (_isExhausted) return;

        if (amount > 0)
        {
            if (Time.time - _lastTickTime > (1f / ticksPerSecond))
            {
                _lastTickTime = Time.time;

                currentStamina = Mathf.Max(currentStamina - amount, 0);
                OnPlayerStaminaChanged?.Invoke(currentStamina);

                if (currentStamina <= 0)
                {
                    _isExhausted = true;
                }
                if (_recoveryCoroutine != null) StopCoroutine(_recoveryCoroutine);
                _recoveryCoroutine = StartCoroutine(RecoveryRoutine());
            }
        }
    }

    public void StayExhausted()
    {
        if (_recoveryCoroutine != null) StopCoroutine(_recoveryCoroutine);
        _recoveryCoroutine = StartCoroutine(RecoveryRoutine());
    }

    private IEnumerator RecoveryRoutine()
    {
        yield return new WaitForSeconds(recoveryDelay);

        while (currentStamina < maxStamina)
        {
            currentStamina = Mathf.MoveTowards(currentStamina, maxStamina, recoverySpeed * Time.deltaTime);
            OnPlayerStaminaChanged?.Invoke(currentStamina);
            yield return null;
        }
        _isExhausted = false;
        _recoveryCoroutine = null;
    }

    public bool IsEnoughStamina(float amount)
    {
        return !_isExhausted || currentStamina >= amount;
    }
}