using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CharacterStamina : CharacterStat
{
    [Header("Recovery Settings")]
    [SerializeField] private float _recoveryCooldown;
    [SerializeField] private float _recoverySpeed;

    private CancellationTokenSource _recoveryCts;
    private bool _isExhausted;

    public override void Decrease(float reducingValue)
    {
        if (_isExhausted) return;

        if (reducingValue > 0)
        {
            _currentValue = Mathf.Max(_currentValue - reducingValue, 0);

            NotifyValueChanged(_currentValue);

            if (_currentValue <= 0) _isExhausted = true;

            _recoveryCts?.Cancel();
            _recoveryCts = new CancellationTokenSource();

            RecoveryRoutineAsync(_recoveryCts.Token).Forget();
        }
    }

    public bool IsEnoughStamina(float amount)
    {
        return !_isExhausted && _currentValue >= amount;
    }

    private async UniTaskVoid RecoveryRoutineAsync(CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(_recoveryCooldown), cancellationToken: cancellationToken);

            while (_currentValue < MaxValue)
            {
                _currentValue = Mathf.MoveTowards(_currentValue, MaxValue, _recoverySpeed * Time.deltaTime);
                NotifyValueChanged(_currentValue);

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cancellationToken);
            }

            _isExhausted = false;
        }
        catch (System.OperationCanceledException)
        {
        }
    }

    private void OnDestroy()
    {
        _recoveryCts?.Cancel();
        _recoveryCts?.Dispose();
    }
}