using UnityEngine;

public abstract class CharacterStat : MonoBehaviour
{
    public event System.Action<float> OnValueChanged;
    public event System.Action OnValueExhausted;

    [Header("Main")]
    [SerializeField] protected float _currentValue;
    [SerializeField] protected float _maxValue;

    public virtual void Decrease(float reducingValue)
    {
        if(reducingValue > 0)
        {
            if(_currentValue > reducingValue)
            {
                _currentValue -= reducingValue;
                OnValueChanged?.Invoke(_currentValue);
            }
            else
            {
                HandleEmptyValue();
            }
        }
    }

    public virtual void Increase(float increasingValue)
    {
        if (increasingValue > 0)
        {
            if (_currentValue + increasingValue > _maxValue)
            {
                _currentValue = _maxValue;
            }
            else
            {
                _currentValue += increasingValue;
            }
            OnValueChanged?.Invoke(_currentValue);
        }
    }

    protected virtual void HandleEmptyValue()
    {
        OnValueExhausted?.Invoke();
    }

    public float GetCurrentValue() { return _currentValue; }

    public float GetMaxValue() { return _maxValue; }

    public void NotifyValueChanged(float value)
    {
        OnValueChanged?.Invoke(value);
    }
}
