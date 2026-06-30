using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class StatsProgressBar : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] protected Image _fillImage;
    [SerializeField] private float _smoothSpeed;

    [Header("Visibility")]
    [SerializeField] private bool _autoHide;
    [SerializeField] private float _hideCooldown;
    [SerializeField] private float _hideDuration;

    [Header("References")]
    [SerializeField] private CharacterStat _characterStat;

    private CanvasGroup _canvasGroup;
    private Coroutine _hideCoroutine;
    private float _defaultAlpha;
    private float _targetFill;

    private void SetValue(float currentValue)
    {
        _targetFill = Mathf.Clamp01(currentValue / _characterStat.MaxValue);

        if (_autoHide)
        {
            ShowBar();
            ResetHideTimer();
        }
    }

    private void ShowBar()
    {
        if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);
        _canvasGroup.alpha = 1f;
    }

    private void ResetHideTimer()
    {
        if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);
        _hideCoroutine = StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(_hideCooldown);

        float startAlpha = _canvasGroup.alpha;
        float elapsed = 0;

        while (elapsed < _hideDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, _defaultAlpha, elapsed / _hideDuration);
            yield return null;
        }

        _canvasGroup.alpha = 0;
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_fillImage == null) _fillImage = GetComponent<Image>();

        _targetFill = _fillImage.fillAmount;
        _defaultAlpha = _canvasGroup.alpha;
    }

    private void Update()
    {
        if (!Mathf.Approximately(_fillImage.fillAmount, _targetFill))
        {
            _fillImage.fillAmount = Mathf.Lerp(_fillImage.fillAmount, _targetFill, Time.deltaTime * _smoothSpeed);
        }
    }

    private void OnEnable()
    {
        _characterStat.OnValueChanged += SetValue;
    }

    private void OnDisable()
    {
        _characterStat.OnValueChanged -= SetValue;
    }
}