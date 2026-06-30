using Zenject;
using UnityEngine;
using System.Collections;

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

    private Coroutine _cooldownCoroutine;

    private bool _isRunning = false;

    private void TryRun()
    {
        if (_groundCheck.IsGrounded)
        {
            if (_characterStamina.IsEnoughStamina(_staminaCost))
            {
                if (_characterEngine.IsMoving() && _cooldownCoroutine == null)
                {
                    _isRunning = true;
                    _cooldownCoroutine = StartCoroutine(ReducingDelay());
                    OnStartRunning?.Invoke();
                }
            }
            else CancelRun();
        }
    }

    private void CancelRun() 
    {
        _isRunning = false;

        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
        }

        _cooldownCoroutine = null;
        OnEndRunning?.Invoke();
    }

    private IEnumerator ReducingDelay()
    {
        while (_isRunning)
        {
            yield return new WaitForFixedUpdate();
            _characterStamina.Decrease(_staminaCost);
        }
        _cooldownCoroutine = null;
    }

    private void OnEnable()
    {
        _inputProvider.OnSprintStarted += TryRun;
        _inputProvider.OnSprintReleased += CancelRun;
    }

    private void OnDisable()
    {
        _inputProvider.OnSprintStarted -= TryRun;
        _inputProvider.OnSprintReleased -= CancelRun;
    }
}