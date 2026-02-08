using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public abstract class ProgressBar : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] protected Image fillImage;
    [SerializeField] protected float maxValue = 100f;
    [SerializeField] private float smoothSpeed;

    [Header("Visibility")]
    [SerializeField] private bool autoHide = false;
    [SerializeField] private float idleTimeBeforeHide = 2f;
    [SerializeField] private float fadeDuration = 0.5f;

    private CanvasGroup _canvasGroup;
    private Coroutine _hideCoroutine;
    private float _defaultAlpha;
    private float _targetFill;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (fillImage == null) fillImage = GetComponent<Image>();

        _targetFill = fillImage.fillAmount;
        _defaultAlpha = _canvasGroup.alpha;
    }

    protected virtual void Update()
    {
        if (!Mathf.Approximately(fillImage.fillAmount, _targetFill))
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, _targetFill, Time.deltaTime * smoothSpeed);
        }
    }

    public virtual void SetValue(float currentValue)
    {
        _targetFill = Mathf.Clamp01(currentValue / GetMaxValue());

        if (autoHide)
        {
            ShowBar();
            ResetHideTimer();
        }
    }
    protected abstract float GetMaxValue();
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
        yield return new WaitForSeconds(idleTimeBeforeHide);

        float startAlpha = _canvasGroup.alpha;
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, _defaultAlpha, elapsed / fadeDuration);
            yield return null;
        }

        _canvasGroup.alpha = 0;
    }
}