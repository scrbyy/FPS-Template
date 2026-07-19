using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CharacterRun : MonoBehaviour
{
    public event System.Action OnStartRunning;
    public event System.Action OnEndRunning;

    [SerializeField] private float _staminaCost;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;
    [SerializeField] private CharacterStamina _characterStamina;

    [Inject] private IGroundChecker _groundCheck;
    [Inject] private IMovementInputProvider _inputProvider;
    [Inject] private CharacterCollisionHandler _characterCollisionHandler;

    private CancellationTokenSource _runCts;
    private bool _isRunning = false;

    private void OnEnable()
    {
        _inputProvider.OnSprintStarted += TryRun;
        _inputProvider.OnSprintReleased += CancelRun;
    }

    private void OnDisable()
    {
        _inputProvider.OnSprintReleased -= CancelRun;
        _inputProvider.OnSprintStarted -= TryRun;

        CancelRun();
    }

    private void TryRun()
    {
        if (!_groundCheck.IsGrounded) return;

        if (!_characterStamina.IsEnoughStamina(_staminaCost))
        {
            CancelRun();
            return;
        }

        if (!_characterEngine.IsMoving()) return;
        if (_isRunning) return;
        if (_characterCollisionHandler.IsCollisionedBySide()) return;

        _isRunning = true;

        _runCts?.Cancel();
        _runCts?.Dispose();
        _runCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

        ReduceStaminaAsync(_runCts.Token).Forget();

        OnStartRunning?.Invoke();
    }

    private void CancelRun()
    {
        if (!_isRunning) return;

        _isRunning = false;

        if (_runCts != null)
        {
            _runCts.Cancel();
            _runCts.Dispose();
            _runCts = null;
        }

        OnEndRunning?.Invoke();
    }

    private async UniTaskVoid ReduceStaminaAsync(CancellationToken cancellationToken)
    {
        while (_isRunning)
        {
            bool isCanceled = await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken).SuppressCancellationThrow();

            if (isCanceled) break;

            _characterStamina.Decrease(_staminaCost);

            if (!_characterStamina.IsEnoughStamina(_staminaCost))
            {
                CancelRun();
                break;
            }
        }
    }
}