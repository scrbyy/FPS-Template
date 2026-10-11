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

    private CanvasGroup _canvasGroup;
    private float _targetFill;
    private float _previousTargetFill;

    private CancellationTokenSource _hideCts;
    private CancellationTokenSource _fillCts;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_fillImage == null) _fillImage = GetComponent<Image>();
        _targetFill = _fillImage.fillAmount;
        _previousTargetFill = _targetFill;
    }

    private void Start()
    {
        _canvasGroup.alpha = _defaultAlpha;
    }

    private void SetValue(float currentValue)
    {
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

        SmoothFillAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid SmoothFillAsync(CancellationToken cancellationToken)
    {
        _fillCts?.Cancel();
        _fillCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            while (!Mathf.Approximately(_fillImage.fillAmount, _targetFill))
            {
                _fillImage.fillAmount = Mathf.Lerp(_fillImage.fillAmount, _targetFill, 1f - Mathf.Exp(-_smoothSpeed * Time.deltaTime));

                await UniTask.Yield(PlayerLoopTiming.Update, _fillCts.Token);
            }

            _fillImage.fillAmount = _targetFill;
        }
        catch (System.OperationCanceledException)
        { }
    }

    private void ShowBar()
    {
        _hideCts?.Cancel();
        _canvasGroup.alpha = 1f;
    }

    private async UniTaskVoid RestartHideTimerAsync(CancellationToken cancellationToken)
    {
        _hideCts?.Cancel();
        _hideCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

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
        { }
    }

    private void OnEnable()
    {
        _characterStat.OnValueChanged += SetValue;
    }

    private void OnDisable()
    {
        _characterStat.OnValueChanged -= SetValue;

        _hideCts?.Cancel();
        _fillCts?.Cancel();
    }

    private void OnDestroy()
    {
        _hideCts?.Dispose();
        _fillCts?.Dispose();
    }
}