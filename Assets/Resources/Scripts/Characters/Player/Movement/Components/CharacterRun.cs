using Zenject;
using UnityEngine;
using System.Collections;

public class CharacterRun : MonoBehaviour
{
    public event System.Action OnStartRunning;
    public event System.Action OnEndRunning;

    [SerializeField] private float _runSpeed;
    [SerializeField] private float _staminaCost;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;
    [SerializeField] private CharacterStamina _playerStamina;
    [SerializeField] private CharacterMovement _playerMovement;

    [Inject] private IGroundChecker _groundChecker;
    [Inject] private IMovementInputProvider _inputProvider;

    private Coroutine _cooldownCoroutine;

    private bool _isRunning = false;

    private void TryRun()
    {
        if (_groundChecker.IsGrounded)
        {
            if (_playerStamina.IsEnoughStamina(_staminaCost))
            {
                if (_characterEngine.IsMoving() && _cooldownCoroutine == null)
                {
                    _isRunning = true;
                    _cooldownCoroutine = StartCoroutine(ReducingDelay());
                    _playerMovement.SetTargetSpeed(_runSpeed);
                    OnStartRunning?.Invoke();
                }
            }
            else CancelRun();
        }
    }

    private void CancelRun() 
    {
        _isRunning = false;
        _playerMovement.ResetSpeed(); 
        StopCoroutine(ReducingDelay());
        _cooldownCoroutine = null;
        OnEndRunning?.Invoke();
    }

    private IEnumerator ReducingDelay()
    {
        while (_isRunning)
        {
            yield return new WaitForFixedUpdate();
            _playerStamina.Decrease(_staminaCost);
        }
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