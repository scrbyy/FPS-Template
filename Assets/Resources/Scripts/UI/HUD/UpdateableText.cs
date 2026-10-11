using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public abstract class UpdateableText : MonoBehaviour
{
    [SerializeField] protected float _updateInterval;

    protected TMP_Text _text;
    private float _accumulatedTime;

    protected virtual void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    protected virtual void Start()
    {
        UpdateText();
    }

    private void Update()
    {
        if (_updateInterval <= 0f)
        {
            UpdateText();
            return;
        }

        _accumulatedTime += Time.unscaledDeltaTime;

        if (_accumulatedTime >= _updateInterval)
        {
            _accumulatedTime = 0f;
            UpdateText();
        }
    }

    protected abstract void UpdateText();
}