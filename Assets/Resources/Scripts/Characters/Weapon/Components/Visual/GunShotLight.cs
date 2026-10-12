using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GunShotLight : MonoBehaviour
{
    [SerializeField] private float _lightDuration;
    [SerializeField] private float _fadeDuration;

    [Header("References")]
    [SerializeField] private Light _light;
    [SerializeField] private Gun _gun;

    private CancellationTokenSource _cts;
    private float _initialIntensity;

    private void Awake()
    {
        if (_light != null)
        {
            _initialIntensity = _light.intensity;
            _light.gameObject.SetActive(true);
            _light.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_gun != null) _gun.OnAttack += ShowLight;
    }

    private void OnDisable()
    {
        if (_gun != null) _gun.OnAttack -= ShowLight;
        CancelAndResetToken();

        if (_light != null)
        {
            _light.enabled = false;
            _light.intensity = _initialIntensity;
        }
    }

    private void OnDestroy()
    {
        _cts?.Dispose();
    }

    private void ShowLight()
    {
        CancelAndResetToken();
        _cts = new CancellationTokenSource();

        HandleLightAsync(_cts.Token).Forget();
    }

    private async UniTaskVoid HandleLightAsync(CancellationToken cancellationToken)
    {
        _light.enabled = true;
        _light.intensity = _initialIntensity;

        bool completedSuccessfully = false;

        try
        {
            if (_lightDuration > 0f)
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(_lightDuration), cancellationToken: cancellationToken);
            }

            if (_fadeDuration > 0f)
            {
                float elapsedTime = 0f;
                while (elapsedTime < _fadeDuration)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    elapsedTime += Time.deltaTime;
                    float progress = elapsedTime / _fadeDuration;
                    _light.intensity = Mathf.Lerp(_initialIntensity, 0f, progress);

                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }
            }

            completedSuccessfully = true;
        }
        catch (System.OperationCanceledException)
        { }
        finally
        {
            if (completedSuccessfully && _light != null)
            {
                _light.enabled = false;
                _light.intensity = _initialIntensity;
            }
        }
    }

    private void CancelAndResetToken()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}