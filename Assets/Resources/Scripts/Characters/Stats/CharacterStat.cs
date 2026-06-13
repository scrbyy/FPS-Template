using UnityEngine;

public abstract class CharacterStat : MonoBehaviour
{
    public event System.Action<float> OnValueChanged;
    public event System.Action OnValueExhausted;

    public float CurrentValue() => _currentValue;
    public float MaxValue() => _maxValue;

    [Header("Main")]
    [SerializeField] protected float _currentValue;
    [SerializeField] protected float _maxValue;



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

    public void NotifyValueChanged(float value)
    {
        OnValueChanged?.Invoke(value);
    }

    protected virtual void HandleEmptyValue()
    {
        OnValueExhausted?.Invoke();
    }
}
