using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(CanvasGroup))]
public class StatProgressBar : MonoBehaviour
{
    [Header("Fill Settings")]
    [SerializeField] private float _smoothSpeed;

    [Header("Visibility")]
    [SerializeField, Range(0, 1)] private float _defaultAlpha;
    [SerializeField] private bool _autoHide;
    [SerializeField] private float _hideCooldown;
    [SerializeField] private float _hideDuration;

    [Header("References")]
    [SerializeField] private CharacterStat _characterStat;
    [SerializeField] protected Image _fillImage;

    [SerializeField] private CanvasGroup _canvasGroup;
    private float _targetFill;
    private float _previousTargetFill;

    private CancellationTokenSource _hideCts;
    private CancellationTokenSource _fillCts;

    private bool _isFilling;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_fillImage == null) _fillImage = GetComponent<Image>();

        if (_characterStat != null)
        {
            _targetFill = Mathf.Clamp01(_characterStat.CurrentValue / _characterStat.MaxValue);
            _fillImage.fillAmount = _targetFill;
            _previousTargetFill = _targetFill;
        }

        _canvasGroup.alpha = _defaultAlpha;
    }

    private void OnEnable()
    {
        if (_characterStat != null)
        {
            _characterStat.OnValueChanged += SetValue;
        }
    }

    private void OnDisable()
    {
        if (_characterStat != null)
        {
            _characterStat.OnValueChanged -= SetValue;
        }

        CancelAndDisposeTokens();
    }

    private void SetValue(float currentValue)
    {
        if (_characterStat == null) return;

        _targetFill = Mathf.Clamp01(currentValue / _characterStat.MaxValue);

        if (_autoHide)
        {
            ShowBar();

            if (!Mathf.Approximately(_targetFill, _previousTargetFill))
            {
                RestartHideTimerAsync(this.GetCancellationTokenOnDestroy()).Forget();
                _previousTargetFill = _targetFill;
            }
        }

        if (!_isFilling)
        {
            SmoothFillAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }

    private async UniTaskVoid SmoothFillAsync(CancellationToken cancellationToken)
    {
        if (_isFilling) return;
        _isFilling = true;

        SetupFillCts(cancellationToken);

        try
        {
            while (!Mathf.Approximately(_fillImage.fillAmount, _targetFill))
            {
                _fillImage.fillAmount = Mathf.Lerp(
                    _fillImage.fillAmount,
                    _targetFill,
                    1f - Mathf.Exp(-_smoothSpeed * Time.deltaTime)
                );

                await UniTask.Yield(PlayerLoopTiming.Update, _fillCts.Token);
            }

            _fillImage.fillAmount = _targetFill;
        }
        catch (System.OperationCanceledException)
        {
        }
        finally
        {
            _isFilling = false;
        }
    }

    private void ShowBar()
    {
        _hideCts?.Cancel();
        _hideCts?.Dispose();
        _hideCts = null;

        _canvasGroup.alpha = 1f;
    }

    private async UniTaskVoid RestartHideTimerAsync(CancellationToken cancellationToken)
    {
        _hideCts?.Cancel();
        _hideCts?.Dispose();

        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _hideCts = linkedCts;

        try
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(_hideCooldown), ignoreTimeScale: false, cancellationToken: _hideCts.Token);

            float startAlpha = _canvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < _hideDuration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, _defaultAlpha, elapsed / _hideDuration);
                await UniTask.Yield(PlayerLoopTiming.Update, _hideCts.Token);
            }

            _canvasGroup.alpha = _defaultAlpha;
        }
        catch (System.OperationCanceledException)
        {
        }
    }

    private void SetupFillCts(CancellationToken baseToken)
    {
        _fillCts?.Cancel();
        _fillCts?.Dispose();
        _fillCts = CancellationTokenSource.CreateLinkedTokenSource(baseToken);
    }

    private void CancelAndDisposeTokens()
    {
        if (_fillCts != null)
        {
            _fillCts.Cancel();
            _fillCts.Dispose();
            _fillCts = null;
        }

        if (_hideCts != null)
        {
            _hideCts.Cancel();
            _hideCts.Dispose();
            _hideCts = null;
        }

        _isFilling = false;
    }

    private void OnDestroy()
    {
        CancelAndDisposeTokens();
    }
}