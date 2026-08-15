using UnityEngine;

public abstract class CharacterStat : MonoBehaviour
{
    public event System.Action<float> OnValueChanged;
    public event System.Action OnValueExhausted;

    public float MaxValue => _maxValue;

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
        else Debug.Log("Increasing value is negative!");
    }

    public virtual void Decrease(float decreasingValue)
    {
        if(decreasingValue > 0)
        {
            if(_currentValue > decreasingValue)
            {
                _currentValue -= decreasingValue;
                OnValueChanged?.Invoke(_currentValue);
            }
            else
            {
                _currentValue = 0;  
                HandleEmptyValue();
            }
        }
        else Debug.Log("Decreasing value is negative!");
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