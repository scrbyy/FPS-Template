using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class StatProgressBar : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] protected Image fillImage;
    [SerializeField] protected float maxValue;
    [SerializeField] private float smoothSpeed;

    [Header("Visibility")]
    [SerializeField] private bool autoHide;
    [SerializeField] private float hideCooldown;
    [SerializeField] private float hideDuration;

    [Header("References")]
    [SerializeField] private CharacterStat _playerStat;

    private CanvasGroup _canvasGroup;
    private Coroutine _hideCoroutine;
    private float _defaultAlpha;
    private float _targetFill;

    private void OnEnable()
    {
        _playerStat.OnValueChanged += SetValue;
    }

    private void OnDisable()
    {
        _playerStat.OnValueChanged -= SetValue;
    }


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (fillImage == null) fillImage = GetComponent<Image>();

        _targetFill = fillImage.fillAmount;
        _defaultAlpha = _canvasGroup.alpha;
    }

    private void Update()
    {
        if (!Mathf.Approximately(fillImage.fillAmount, _targetFill))
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, _targetFill, Time.deltaTime * smoothSpeed);
        }
    }

    private void SetValue(float currentValue)
    {
        _targetFill = Mathf.Clamp01(currentValue / _playerStat.MaxValue());

        if (autoHide)
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
        yield return new WaitForSeconds(hideCooldown);

        float startAlpha = _canvasGroup.alpha;
        float elapsed = 0;

        while (elapsed < hideDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, _defaultAlpha, elapsed / hideDuration);
            yield return null;
        }

        _canvasGroup.alpha = 0;
    }
}